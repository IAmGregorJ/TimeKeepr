namespace TimeKeepr.Domain.Models
{
    public class User : DomainObject
    {
        public string EMail { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; } //hash
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkPlace { get; set; }
        public double HoursPerWeek { get; set; }
        public string Salt { get; set; }
    }
}