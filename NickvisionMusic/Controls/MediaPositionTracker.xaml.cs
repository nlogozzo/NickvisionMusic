/****
* "MediaPositionTracker.xaml.cs" - The MediaPositionTracker class
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

using System.Windows;
using System.Windows.Controls;

namespace NickvisionMusic.Controls
{
    /// <summary>
    /// The MediaPositionTracker class
    /// </summary>
    public partial class MediaPositionTracker : UserControl
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(double), typeof(MediaPositionTracker), new PropertyMetadata(0.0));
        public static readonly DependencyProperty PositionStringProperty = DependencyProperty.Register("PositionString", typeof(string), typeof(MediaPositionTracker), new PropertyMetadata("00:00:00"));
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(double), typeof(MediaPositionTracker), new PropertyMetadata(0.0));
        public static readonly DependencyProperty DurationStringProperty = DependencyProperty.Register("DurationString", typeof(string), typeof(MediaPositionTracker), new PropertyMetadata("00:00:00"));
        
        public MediaPositionTracker()
        {
            InitializeComponent();
        }

        public double Position
        {
            get => (double)GetValue(PositionProperty);

            set => SetValue(PositionProperty, value);
        }

        public string PositionString
        {
            get => (string)GetValue(PositionStringProperty);

            set => SetValue(PositionStringProperty, value);
        }

        public double Duration
        {
            get => (double)GetValue(DurationProperty);

            set => SetValue(DurationProperty, value);
        }

        public string DurationString
        {
            get => (string)GetValue(DurationStringProperty);

            set => SetValue(DurationStringProperty, value);
        }
    }
}
