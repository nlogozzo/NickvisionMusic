/****
* "MediaExtensions.cs" - Extensions for converting types to media readable strings
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

using System;

namespace NickvisionMusic.Extensions
{
    /// <summary>
    /// Extensions for converting types to media readable strings
    /// </summary>
    public static class MediaExtensions
    {
        /// <summary>
        /// Converts a time to a human readable duration
        /// </summary>
        /// <param name="duration">The duration of the file</param>
        /// <returns>A human readable string representing the duration (HH:MM:SS)</returns>
        public static string DurationToString(this TimeSpan duration)
        {
            var seconds = (uint)duration.TotalSeconds;
            var minutes = seconds / 60;
            var hours = minutes / 60;
            return $"{hours:00}:{(minutes % 60):00}:{(seconds % 60):00}";
        }
    }
}
