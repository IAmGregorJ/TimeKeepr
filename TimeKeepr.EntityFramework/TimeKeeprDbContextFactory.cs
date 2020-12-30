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

using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TimeKeepr.EntityFramework
{
    public class TimeKeeprDbContextFactory : IDesignTimeDbContextFactory<TimeKeeprDbContext>
    {
        public TimeKeeprDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<TimeKeeprDbContext>();

            //start getting ready for deployment - saving the db in AppData
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var userFilePath = Path.Combine(localAppData, "TimeKeepr");

            //create the folder if it doesn't exist yet
            if (!Directory.Exists(userFilePath))
                Directory.CreateDirectory(userFilePath);

            //if the db isn't there yet
            //copy the db file from deployment location to the folder
            var sourceFilePath = "timeKeeperDB.db";
            var destFilePath = Path.Combine(userFilePath, "timeKeeperDB.db");
            if (!File.Exists(destFilePath))
                File.Copy(sourceFilePath, destFilePath);

            options.UseSqlite($"Data Source={destFilePath};");

            //options.UseSqlite(@"Data Source=.\timeKeeperDB.db;");

            return new TimeKeeprDbContext(options.Options);
        }
    }
}