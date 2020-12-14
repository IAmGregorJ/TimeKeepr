using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeKeepr.Domain.Models;
using TimeKeepr.Domain.Services;

namespace TimeKeepr.EntityFramework.Services
{
    public class DataService<T> : IDataService<T> where T : DomainObject
    {
        private readonly TimeKeeprDbContextFactory _contextFactory;

        public DataService(TimeKeeprDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using TimeKeeprDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<T> createdResult = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using TimeKeeprDbContext context = _contextFactory.CreateDbContext();
            T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<T> Get(int id)
        {
            using TimeKeeprDbContext context = _contextFactory.CreateDbContext();
            T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
            return entity;
        }

        public async Task<User> GetByUserName(string username)
        {
            using TimeKeeprDbContext context = _contextFactory.CreateDbContext();
            User entity = await context.Users
                .Where(a => a.UserName == username)
                .FirstOrDefaultAsync();
            return entity;
        }

        public async Task<EventCategory> GetByCategoryName(string category)
        {
            using TimeKeeprDbContext context = _contextFactory.CreateDbContext();
            EventCategory entity = await context.EventCategories
                .Where(a => a.Category == category)
                .FirstOrDefaultAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using TimeKeeprDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<T> entities = await context.Set<T>().ToListAsync();
            return entities;
        }

        public async Task<T> Update(int id, T entity)
        {
            using TimeKeeprDbContext context = _contextFactory.CreateDbContext();
            entity.Id = id;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}