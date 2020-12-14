using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TimeKeepr.EntityFramework
{
    public class TimeKeeprDbContextFactory : IDesignTimeDbContextFactory<TimeKeeprDbContext>
    {
        public TimeKeeprDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<TimeKeeprDbContext>();
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TimeKeeprDB;Trusted_Connection=True;");

            return new TimeKeeprDbContext(options.Options);
        }
    }
}