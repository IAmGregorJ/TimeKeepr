namespace TimeKeepr.Domain.Models
{
    public class User : DomainObject
    {
        public string EMail { get; set; } //is this needed?
        public string UserName { get; set; }
        public string PasswordHash { get; set; } //hash - should be byte[]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkPlace { get; set; }
        public double HoursPerWeek { get; set; }
        public string Salt { get; set; } //I should really set this to byte[]

        //public double StartSaldo { get; set; } <- used in case of user starting over with a fresh installation
    }
}