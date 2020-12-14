using System;

namespace TimeKeepr.Domain.Models
{
    public class HappeningsFilteredYearCategory
    {
        public string Category { get; set; } //user defines the categories that are not "work"
        public double TimeInHours { get; set; } // <-- from start/stop time
        public DateTime EventDate { get; set; }
        public int Year { get; set; }
    }
}