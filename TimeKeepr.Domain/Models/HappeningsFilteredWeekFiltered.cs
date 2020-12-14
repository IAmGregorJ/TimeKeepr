using System;

namespace TimeKeepr.Domain.Models
{
    public class HappeningsFilteredWeekFiltered
    {
        public string Category { get; set; } //user defines the categories that are not "work"
        public double TimeInHours { get; set; } // <-- from start/stop time
        public DateTime EventDate { get; set; }
        public int WeekNr { get; set; }
        public int Year { get; set; }
        public double DifferenceBetween { get; set; }
    }
}