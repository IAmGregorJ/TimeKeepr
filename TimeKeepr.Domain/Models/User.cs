namespace TimeKeepr.Domain.Models
{
    public class User : DomainObject
    {
        public string EMail { get; set; } //is this needed?
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkPlace { get; set; }
        public double HoursPerWeek { get; set; }
        public double PreviousSaldo { get; set; }
        public string Salt { get; set; } //I should really set this to byte[]
        public string PasswordHash { get; set; } //hash - should be byte[]
    }
}