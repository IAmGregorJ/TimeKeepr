using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TimeKeepr.EntityFramework
{
    public class TimeKeeprDbContextFactory : IDesignTimeDbContextFactory<TimeKeeprDbContext>
    {
        public TimeKeeprDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<TimeKeeprDbContext>();

            //TRY THIS LATER
            //var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"TimeKeepr\timekeeprdatabase.db");
            //options.UseSqlite(@"Data Source={databasepath};");

            options.UseSqlite(@"Data Source=C:\Users\grggm\source\repos\TimeKeepr\TimeKeepr.WPF\timeKeeperDB.db;");

            return new TimeKeeprDbContext(options.Options);
        }
    }
}