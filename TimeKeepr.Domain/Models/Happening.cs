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

namespace TimeKeepr.Domain.Models
{
    public class Happening : DomainObject
    {
        public string Category { get; set; } //user defines the categories that are not "work"
        public string UserName { get; set; }
        public bool IsMeeting { get; set; }
        public double IsMeetingHours { get; set; }
        public double TimeInHours { get; set; } // <-- from start/stop time
        public DateTime EventDate { get; set; }
        public int Year { get; set; }
        public int WeekNr { get; set; } // <-- from class library
    }
}