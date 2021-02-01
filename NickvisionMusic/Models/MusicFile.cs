/****
* "MusicFile.cs" - The MusicFile class
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

using NickvisionMusic.Extensions;
using System;
using TagLib;

namespace NickvisionMusic.Models
{
    /// <summary>
    /// The MusicFile class
    /// </summary>
    public class MusicFile
    {
        private File _musicFile;

        public string Path { get; private set; }

        /// <summary>
        /// Constructs a MusicFile
        /// </summary>
        /// <param name="path">The path to the music file</param>
        public MusicFile(string path)
        {
            Path = path;
            _musicFile = File.Create(path);
        }

        public string Filename => System.IO.Path.GetFileName(Path);

        public string Title => _musicFile.Tag.Title;

        public string Artist => _musicFile.Tag.FirstPerformer;

        public string Album => _musicFile.Tag.Album;

        public uint Year => _musicFile.Tag.Year;

        public uint Track => _musicFile.Tag.Track;

        public string AlbumArtist => _musicFile.Tag.FirstAlbumArtist;

        public string Genre => _musicFile.Tag.FirstGenre;

        public string Comment => _musicFile.Tag.Comment;

        public TimeSpan Duration => _musicFile.Properties.Duration;

        public string DurationAsString => Duration.DurationToString();
    }
}
