/****
* "EditMusicSourcesDialogViewModel.cs" - The ViewModel for EditMusicSources Dialog
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
using Nickvision.MVVM.Commands;
using Nickvision.MVVM.Services;
using NickvisionMusic.Models;
using System.Collections.ObjectModel;

namespace NickvisionMusic.ViewModels
{
    /// <summary>
    /// The ViewModel for EditMusicSources Dialog
    /// </summary>
    public class EditMusicSourcesDialogViewModel : ViewModelBase
    {
        private IIODialogService _ioDialogService;
        private MusicLibrary _musicLibrary;
        private string _selectedPath;

        public ObservableCollection<string> FolderPaths => new ObservableCollection<string>(_musicLibrary.FolderPaths);
        public DelegateCommand<object> AddCommand { get; private set; }
        public DelegateCommand<object> DeleteCommand { get; private set; }
        public DelegateCommand<ICloseable> CloseCommand { get; private set; }

        /// <summary>
        /// Constructs the viewmodel
        /// </summary>
        /// <param name="ioDialogService">The IIODialogService</param>
        /// <param name="musicLibrary">The MusicLibrary to store the sources in</param>
        public EditMusicSourcesDialogViewModel(IIODialogService ioDialogService, MusicLibrary musicLibrary)
        {
            Title = "Edit Music Sources";
            _ioDialogService = ioDialogService;
            _musicLibrary = musicLibrary;
            AddCommand = new DelegateCommand<object>(Add);
            DeleteCommand = new DelegateCommand<object>(Delete, () => SelectedPath != null);
            CloseCommand = new DelegateCommand<ICloseable>(Close);
        }

        public string SelectedPath
        {
            get => _selectedPath;

            set
            {
                SetProperty(ref _selectedPath, value);
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Adds a folder source to the MusicLibrary
        /// </summary>
        /// <param name="parameter"></param>
        private void Add(object parameter)
        {
            var folderPath = _ioDialogService.OpenFolderDialog("Add Music Folder");
            if(!string.IsNullOrEmpty(folderPath))
            {
                _musicLibrary.FolderPaths.Add(folderPath);
                OnPropertyChanged("FolderPaths");
            }
        }

        /// <summary>
        /// Deletes a folder source from the MusicLibrary
        /// </summary>
        /// <param name="parameter"></param>
        private void Delete(object parameter)
        {
            _musicLibrary.FolderPaths.Remove(SelectedPath);
            OnPropertyChanged("FolderPaths");
        }

        /// <summary>
        /// Closes the EditMusicSources Dialog
        /// </summary>
        /// <param name="parameter"></param>
        private void Close(ICloseable window)
        {
            window.Close();
        }
    }
}
