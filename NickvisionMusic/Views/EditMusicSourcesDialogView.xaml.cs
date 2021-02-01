/****
* "EditMusicSourcesDialogView.xaml.cs" - The EditMusicSourcesDialog class
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
using System.Windows;

namespace NickvisionMusic.Views
{
    /// <summary>
    /// The EditMusicSourcesDialog class
    /// </summary>
    public partial class EditMusicSourcesDialogView : Window, ICloseable
    {
        /// <summary>
        /// Constructs and initializes an EditMusicSources Dialog
        /// </summary>
        public EditMusicSourcesDialogView() => InitializeComponent();
    }
}
