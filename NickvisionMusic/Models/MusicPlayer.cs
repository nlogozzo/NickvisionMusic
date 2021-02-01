/****
* "MusicPlayer.cs" - The MusicPlayer class
* Copyright (C) 2021 Nicholas Logozzo
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, see <http://www.gnu.org/licenses/>.
****/

using Nickvision.MVVM;
using NickvisionMusic.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Media;

namespace NickvisionMusic.Models
{
    /// <summary>
    /// The MusicPlayer class
    /// </summary>
    public class MusicPlayer : ObservableObject
    {
        private MediaPlayer _musicPlayer;
        private Timer _timer;
        private Random _random;
        private MusicFile _source;
        private bool _isPlaying;
        private bool _isShuffle;
        private bool _isRepeat;
        private List<string> _shuffleList;

        public MusicLibrary MusicLibrary { get; private set; }
        public ObservableCollection<MusicFile> AllMusicFiles => new ObservableCollection<MusicFile>(MusicLibrary.Files);

        /// <summary>
        /// Constructs a MusicPlayer
        /// </summary>
        public MusicPlayer()
        {
            _musicPlayer = new MediaPlayer();
            _timer = new Timer(1000);
            _random = new Random();
            _shuffleList = new List<string>();
            MusicLibrary = new MusicLibrary();
            _isPlaying = false;
            IsShuffle = false;
            IsRepeat = false;
            Volume = 50;
            Source = null;
            _timer.Elapsed += TimerTick;
        }

        public bool IsShuffle
        {
            get => _isShuffle;

            set => SetProperty(ref _isShuffle, value);
        }

        public bool IsRepeat
        {
            get => _isRepeat;

            set => SetProperty(ref _isRepeat, value);
        }

        public double Volume
        {
            get => _musicPlayer.Volume * 100;

            set => _musicPlayer.Volume = value / 100;
        }

        public double Position
        {
            get => _musicPlayer.Position.TotalSeconds;

            set
            {
                _musicPlayer.Position = TimeSpan.FromSeconds(value);
                OnPropertyChanged();
                OnPropertyChanged("PositionString");
            }
        }

        public string PositionString => _musicPlayer.Position.DurationToString();

        public double Duration => Source == null ? 0 : Source.Duration.TotalSeconds;

        public string DurationString => Source == null ? "00:00:00" : Source.DurationAsString;

        public MusicFile Source
        {
            get => _source;

            set
            {
                _source = value;
                if(Source != null)
                {
                    Stop();
                    _musicPlayer.Close();
                    _musicPlayer.Open(new Uri(Source.Path));
                }
                OnPropertyChanged();
                OnPropertyChanged("Duration");
                OnPropertyChanged("DurationString");
            }
        }

        /// <summary>
        /// Rescans the music sources and generates a new music files list
        /// </summary>
        public void ReloadFiles()
        {
            MusicLibrary.ReloadFiles();
            _shuffleList.Clear();
            foreach(var musicFile in MusicLibrary.Files)
            {
                _shuffleList.Add(musicFile.Path);
            }
            Shuffle(_shuffleList);
            OnPropertyChanged("AllMusicFiles");
            OnPropertyChanged("Position");
            OnPropertyChanged("PositionString");
            OnPropertyChanged("Duration");
            OnPropertyChanged("DurationString");
        }

        /// <summary>
        /// Removes the currently playing song from the music player
        /// </summary>
        public void Close()
        {
            Source = null;
            _musicPlayer.Close();
        }

        /// <summary>
        /// Plays the previous song
        /// </summary>
        public void Previous()
        {
            if (IsShuffle && Source != null)
            {
                var previousIndex = _shuffleList.IndexOf(Source.Path) - 1;
                if (previousIndex == -1)
                {
                    previousIndex = 0;
                }
                Source = MusicLibrary.Files.Find(x => x.Path == _shuffleList[previousIndex]);
                Play();
            }
            else if (Source != null)
            {
                var previousIndex = MusicLibrary.Files.IndexOf(Source) - 1;
                if (previousIndex == -1)
                {
                    previousIndex = MusicLibrary.Files.Count - 1;
                }
                Source = MusicLibrary.Files[previousIndex];
                Play();
            }
        }

        /// <summary>
        /// Plays the currently loaded song
        /// </summary>
        public void Play()
        {
            if(!_isPlaying && Source != null)
            {
                _timer.Start();
                _musicPlayer.Play();
                _isPlaying = true;
            }
        }

        /// <summary>
        /// Pauses the currently loaded song
        /// </summary>
        public void Pause()
        {
            if(_isPlaying)
            {
                _musicPlayer.Pause();
                _timer.Stop();
                _isPlaying = false;
                OnPropertyChanged("Position");
                OnPropertyChanged("PositionString");
            }
        }

        /// <summary>
        /// Stops the currently loaded song
        /// </summary>
        public void Stop()
        {
            Position = 0;
            _musicPlayer.Stop();
            _timer.Stop();
            _isPlaying = false;
            OnPropertyChanged("Position");
            OnPropertyChanged("PositionString");
        }

        /// <summary>
        /// Plays the next song
        /// </summary>
        public void Next()
        {
            if (IsShuffle && Source != null)
            {
                var nextIndex = _shuffleList.IndexOf(Source.Path) + 1;
                if(nextIndex == _shuffleList.Count)
                {
                    nextIndex = 0;
                }
                Source = MusicLibrary.Files.Find(x => x.Path == _shuffleList[nextIndex]);
                Play();
            }
            else if (IsRepeat && Source != null)
            {
                Play();
            }
            else if (Source != null)
            {
                var nextIndex = MusicLibrary.Files.IndexOf(Source) + 1;
                if(nextIndex == MusicLibrary.Files.Count)
                {
                    nextIndex = 0;
                }
                Source = MusicLibrary.Files[nextIndex];
                Play();
            }
        }

        /// <summary>
        /// Backs the currently selected song up by 15 seconds
        /// </summary>
        public void FifteenSecondsBack()
        {
            if(Source != null && Position >= 15)
            {
                Position -= 15;
            }
        }

        /// <summary>
        /// Occurs every second when the timer starts to update the song position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            OnPropertyChanged("Position");
            OnPropertyChanged("PositionString");
        }

        /// <summary>
        /// Shuffles a list using the Fisher-Yates algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToShuffle">The list to shuffle</param>
        private void Shuffle<T>(List<T> listToShuffle)
        {
            var n = listToShuffle.Count;
            for(int i = n - 1; i > 0; i--)
            {
                var r = _random.Next(0, i + 1);
                T t = listToShuffle[i];
                listToShuffle[i] = listToShuffle[r];
                listToShuffle[r] = t;
            }
        }
    }
}
