/****
* "MusicLibrary.cs" - The MusicLibrary class
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

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NickvisionMusic.Models
{
    /// <summary>
    /// MusicLibrary
    /// </summary>
    public class MusicLibrary
    {
        public List<string> FolderPaths { get; private set; }
        public List<MusicFile> Files { get; private set; }

        /// <summary>
        /// Constructs a MusicLibrary
        /// </summary>
        public MusicLibrary()
        {
            FolderPaths = new List<string>();
            Files = new List<MusicFile>();
        }

        /// <summary>
        /// Searches the directories of the FolderPaths list and finds all compatible music files and converts then to MusicFiles objects and adds them to the Files list
        /// </summary>
        public void ReloadFiles()
        {
            var extensions = new List<string>() { ".mp3", ".wav", ".wma", ".ogg", ".flac" };
            Files.Clear();
            foreach(var folder in FolderPaths)
            {
                if(Directory.Exists(folder))
                {
                    foreach(var file in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories).Where(path => extensions.Contains(Path.GetExtension(path))))
                    {
                        var musicFile = new MusicFile(file);
                        var listContains = false;
                        foreach(var f in Files)
                        {
                            if(f.Filename == musicFile.Filename)
                            {
                                listContains = true;
                            }
                        }
                        if(listContains != true)
                        {
                            Files.Add(musicFile);
                        }
                    }
                }
            }
            Files.Sort((f1, f2) => string.Compare(f1.Filename, f2.Filename));
        }
    }
}
