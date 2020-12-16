namespace TimeKeepr.Domain.Models
{
    public class EventCategory : DomainObject
    {
        public string Category { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; } //<- who does the category belong to - in the case of several users
    }
}