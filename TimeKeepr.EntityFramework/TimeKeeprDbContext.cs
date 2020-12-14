using Microsoft.EntityFrameworkCore;
using TimeKeepr.Domain.Models;

namespace TimeKeepr.EntityFramework
{
    public class TimeKeeprDbContext : DbContext
    {
        public TimeKeeprDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Happening> Happenings { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
    }
}