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