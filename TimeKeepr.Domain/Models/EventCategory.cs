namespace TimeKeepr.Domain.Models
{
    public class EventCategory : DomainObject
    {
        public string Category { get; set; }
        public bool IsActive { get; set; }
    }
}