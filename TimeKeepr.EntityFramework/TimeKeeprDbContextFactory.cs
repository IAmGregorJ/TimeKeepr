using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TimeKeepr.EntityFramework
{
    public class TimeKeeprDbContextFactory : IDesignTimeDbContextFactory<TimeKeeprDbContext>
    {
        public TimeKeeprDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<TimeKeeprDbContext>();
            //string relativePath = @"timekeeprdatabase.db";
            //string currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            //string absolutePath = System.IO.Path.Combine(currentPath, relativePath);
            //absolutePath = absolutePath.Remove(0, 6);//this code is written to remove file word from absolute path
            //string connectionString = string.Format("Data Source={0}", absolutePath);


            //TRY THIS LATER
            //var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"TimeKeepr\timekeeprdatabase.db");
            //options.UseSqlite(@"Data Source={databasepath};");

            options.UseSqlite(@"Data Source=C:\Users\grggm\source\repos\TimeKeepr\TimeKeepr.WPF\timeKeeperDB.db;");

            return new TimeKeeprDbContext(options.Options);
        }
    }
}