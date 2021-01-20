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

namespace TimeKeepr.Domain.Models
{
    public class FlexTime : DomainObject
    {
        public string UserName
        {
            get; set;
        }

        public int Year
        {
            get; set;
        }

        public int WeekNr
        {
            get; set;
        }

        public double HoursPerWeek
        {
            get; set;
        }

        public double TotalHoursWeek
        {
            get; set;
        }

        public double FlexHours
        {
            get; set;
        }
    }
}
