/****
* "MainWindowViewModel.cs" - The ViewModel for MainWindow
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

using ModernWpf.Controls;
using Nickvision.MVVM;
using Nickvision.MVVM.Commands;
using Nickvision.MVVM.Services;
using Nickvision.Update;
using NickvisionMusic.Models;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace NickvisionMusic.ViewModels
{
    /// <summary>
    /// The ViewModel for MainWindow
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private INotificationService _notificationService;
        private IContentDialogService _contentDialogService;
        private IIODialogService _ioDialogService;
        private IProgressDialogService _progressDialogService;
        private IWindowService _windowService;

        public MusicPlayer MusicPlayer { get; private set; }
        public DelegateCommand<object> ExitCommand { get; private set; }
        public DelegateCommand<object> PreviousCommand { get; private set; }
        public DelegateCommand<MusicFile> PlayPauseCommand { get; private set; }
        public DelegateCommand<object> StopCommand { get; private set; }
        public DelegateCommand<object> NextCommand { get; private set; }
        public DelegateCommand<object> FifteenSecondsBackCommand { get; private set; }
        public DelegateCommand<object> ReloadLibraryCommand { get; private set; }
        public DelegateCommand<object> EditMusicSourcesCommand { get; private set; }
        public DelegateAsyncCommand<object> CheckForUpdatesCommand { get; private set; }
        public DelegateCommand<object> ReportABugCommand { get; private set; }
        public DelegateAsyncCommand<object> ChangelogCommand { get; private set; }
        public DelegateAsyncCommand<object> AboutCommand { get; private set; }

        /// <summary>
        /// Constructs the viewmodel
        /// </summary>
        /// <param name="notificationService">The INotificationService</param>
        /// <param name="contentDialogService">The IContentDialogService</param>
        /// <param name="ioDialogService">The IIODialogService</param>
        /// <param name="progressDialogService">The IProgressDialogService</param>
        /// <param name="windowService">The IWindowService</param>
        public MainWindowViewModel(INotificationService notificationService, IContentDialogService contentDialogService, IIODialogService ioDialogService, IProgressDialogService progressDialogService, IWindowService windowService)
        {
            Title = "Nickvision Music";
            ControlzEx.Theming.ThemeManager.Current.ChangeThemeColorScheme(Application.Current, "Purple");
            _notificationService = notificationService;
            _contentDialogService = contentDialogService;
            _ioDialogService = ioDialogService;
            _progressDialogService = progressDialogService;
            _windowService = windowService;
            MusicPlayer = new MusicPlayer();
            ExitCommand = new DelegateCommand<object>(Exit);
            PreviousCommand = new DelegateCommand<object>(Previous);
            PlayPauseCommand = new DelegateCommand<MusicFile>(PlayPause);
            StopCommand = new DelegateCommand<object>(Stop);
            NextCommand = new DelegateCommand<object>(Next);
            FifteenSecondsBackCommand = new DelegateCommand<object>(FifteenSecondsBack);
            ReloadLibraryCommand = new DelegateCommand<object>(ReloadLibrary);
            EditMusicSourcesCommand = new DelegateCommand<object>(EditMusicSources);
            CheckForUpdatesCommand = new DelegateAsyncCommand<object>(CheckForUpdates);
            ReportABugCommand = new DelegateCommand<object>(ReportABug);
            ChangelogCommand = new DelegateAsyncCommand<object>(Changelog);
            AboutCommand = new DelegateAsyncCommand<object>(About);
            LoadConfig();
        }

        public bool IsLightTheme
        {
            get => ModernWpf.ThemeManager.Current.ApplicationTheme == ModernWpf.ApplicationTheme.Light;

            set
            {
                if (value == true)
                {
                    ModernWpf.ThemeManager.Current.ApplicationTheme = ModernWpf.ApplicationTheme.Light;
                    ControlzEx.Theming.ThemeManager.Current.ChangeThemeBaseColor(Application.Current, "Light");
                    IsDarkTheme = false;
                    UpdateConfig();
                }
                OnPropertyChanged();
            }
        }

        public bool IsDarkTheme
        {
            get => ModernWpf.ThemeManager.Current.ApplicationTheme == ModernWpf.ApplicationTheme.Dark;

            set
            {
                if (value == true)
                {
                    ModernWpf.ThemeManager.Current.ApplicationTheme = ModernWpf.ApplicationTheme.Dark;
                    ControlzEx.Theming.ThemeManager.Current.ChangeThemeBaseColor(Application.Current, "Dark");
                    IsLightTheme = false;
                    UpdateConfig();
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Loads the configuration file and applies its preferences
        /// </summary>
        private void LoadConfig()
        {
            var config = Config.LoadConfig();
            foreach (var folder in config.MusicSources)
            {
                MusicPlayer.MusicLibrary.FolderPaths.Add(folder);
            }
            if (config.IsLightTheme)
            {
                IsLightTheme = true;
            }
            else
            {
                IsDarkTheme = true;
            }
            MusicPlayer.IsShuffle = config.IsShuffle;
            MusicPlayer.IsRepeat = config.IsRepeat;
            ReloadLibrary(null);
        }

        /// <summary>
        /// Updates the configuration file based on the current application's settings
        /// </summary>
        private void UpdateConfig() => Config.SaveConfig(new Config(IsLightTheme, MusicPlayer.MusicLibrary.FolderPaths, MusicPlayer.IsShuffle, MusicPlayer.IsRepeat));

        /// <summary>
        /// Closes the program
        /// </summary>
        private void Exit(object parameter)
        {
            UpdateConfig();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Plays the previous song
        /// </summary>
        /// <param name="parameter"></param>
        private void Previous(object parameter) => MusicPlayer.Previous();

        /// <summary>
        /// Plays or pauses the currently selected song
        /// </summary>
        /// <param name="selectedMusicFile">The MusicFile selected from the DataGrid</param>
        private void PlayPause(MusicFile selectedMusicFile)
        {
            if(MusicPlayer.IsPlaying)
            {
                MusicPlayer.Pause();
            }
            else
            {
                if (selectedMusicFile != null)
                {
                    MusicPlayer.Source = selectedMusicFile;
                }
                MusicPlayer.Play();
            }
        }

        /// <summary>
        /// Stops the currently selected song
        /// </summary>
        /// <param name="parameter"></param>
        private void Stop(object parameter) => MusicPlayer.Stop();

        /// <summary>
        /// Plays the next song
        /// </summary>
        /// <param name="parameter"></param>
        private void Next(object parameter) => MusicPlayer.Next();

        /// <summary>
        /// Backs the currently selected song up by 15 seconds
        /// </summary>
        /// <param name="parameter"></param>
        private void FifteenSecondsBack(object parameter) => MusicPlayer.FifteenSecondsBack();

        /// <summary>
        /// Rescans the music sources and generates a new music files list
        /// </summary>
        /// <param name="parameter"></param>
        private void ReloadLibrary(object parameter)
        {
            MusicPlayer.Stop();
            MusicPlayer.Close();
            _progressDialogService.ShowDialog("Loading music files...", async () => await Task.Run(() => MusicPlayer.ReloadFiles()));
        }

        /// <summary>
        /// Opens a EditMusicSources Dialog to add sources to MusicLibrary
        /// </summary>
        /// <param name="parameter"></param>
        private void EditMusicSources(object parameter)
        {
            _windowService.ShowDialog(new EditMusicSourcesDialogViewModel(_ioDialogService, MusicPlayer.MusicLibrary));
            UpdateConfig();
            ReloadLibrary(null);
        }

        /// <summary>
        /// Checks for updates and if one is available, will prompt the user to automatically update the app
        /// </summary>
        private async Task CheckForUpdates(object parameter)
        {
            var updater = new Updater("https://raw.githubusercontent.com/nlogozzo/NickvisionMusic/main/UpdateConfig.json", Assembly.GetExecutingAssembly().GetName().Version);
            await updater.CheckForUpdateAsync();
            if (updater.UpdateAvaliable)
            {
                var result = await _contentDialogService.ShowAsync($"An update is available V{updater.LatestVersion}. Would you like to update?\nIf yes, Nickvision Music will attempt to download the installer and will close the application to update. Please make sure all work is saved", "Update Available", "No", "Yes");
                if (result == ContentDialogResult.Primary)
                {
                    var updateSuccessful = false;
                    _progressDialogService.ShowDialog("Downloading the update...", async () => updateSuccessful = await updater.Update());
                    if (!updateSuccessful)
                    {
                        await _notificationService.Send("Update failed. Please try again later", "Update Failed", SystemIcons.Application);
                    }
                }
            }
            else
            {
                await _notificationService.Send("No update is available at this time", "No Update Available", SystemIcons.Application);
            }
        }

        /// <summary>
        /// Opens a browser and navigates to this program's new issue page on GitHub
        /// </summary>
        private void ReportABug(object parameter) => Process.Start(new ProcessStartInfo("cmd", $"/c start {"https://github.com/nlogozzo/NickvisionMusic/issues/new"}") { CreateNoWindow = true });

        /// <summary>
        /// Displays information about this program
        /// </summary>
        private async Task Changelog(object parameter) => await _contentDialogService.ShowAsync("- Combined Play and Pause buttons into one Play/Pause button\n- Added support for media keyboard buttons", "What's New?", "OK");

        /// <summary>
        /// Handles when the window closes
        /// </summary>
        private async Task About(object parameter) => await _contentDialogService.ShowAsync($"Nickvision Music V{Assembly.GetExecutingAssembly().GetName().Version}", "About Nickvision Music", "OK");
    }
}
