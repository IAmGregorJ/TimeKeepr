// This file is part of TimeKeepr.
//
// TimeKeepr is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// TimeKeepr is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY - without even the implied warranty of
//
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with TimeKeepr.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;

namespace TimeKeepr.WPF.Helper
{
    [Serializable()]
    class PersistentData
    {
        public string StButtonIsEnabledP { get; set; }
        public string StwButtonIsEnabledP { get; set; }
        public string SpButtonIsEnabledP { get; set; }
        public string SpwButtonIsEnabledP { get; set; }
        public string RegButtonIsEnabledP { get; set; }
        public string RegwButtonIsEnabledP { get; set; }
        public int SelectedHourStWP { get; set; }
        public int SelectedMinuteStWP { get; set; }
        public int SelectedHourSpWP { get; set; }
        public int SelectedMinuteSpWP { get; set; }
        public int SelectedHourStTP { get; set; }
        public int SelectedMinuteStTP { get; set; }
        public int SelectedHourSpTP { get; set; }
        public int SelectedMinuteSpTP { get; set; }
        public DateTime DateWorkP { get; set; }
        public DateTime DateTaskP { get; set; }
        public string CategoryP { get; set; } //Category = SelectedCategory.Category;
        public bool IsMeetingP { get; set; }
    }
}
