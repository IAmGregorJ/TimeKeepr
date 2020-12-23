// This file is part of TimeKeepr.
//
// TimeKeepr is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// TimeKeepr is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY - without even the implied warranty of
//
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with TimeKeepr.  If not, see <https://www.gnu.org/licenses/>.

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

            options.UseSqlite(@"Data Source=.\timeKeeperDB.db;");

            return new TimeKeeprDbContext(options.Options);
        }
    }
}