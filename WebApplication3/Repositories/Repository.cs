using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using WebApplication3.Entities;
using WebApplication3.IRepositories;

namespace WebApplication3.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
       
        private readonly NewDbContext _dbContext;
        public Repository(NewDbContext db)
        {
            _dbContext = db;
        }

        public DbSet<T> Table => _dbContext.Set<T>();
        public async Task<bool> AddAsync(T data)
        {
            var info = await _dbContext.AddAsync(data);
            return info.State == EntityState.Added;
        }

        public  IQueryable<T> GetAll()
        {
           return Table.AsQueryable();
        }

        public async Task<T> GetById(int id) => await Table.FirstOrDefaultAsync(x => x.Id == id);

        //public async Task<T> GetById(int id)
        //{
        //    return await Table.FirstOrDefaultAsync(x => x.Id == id);
        //}

        //{
        //    var data = await Table.FirstOrDefaultAsync(x => x.Id == id);
        //    return data;
        //    //var data1 = Table.AsQueryable();
        //    //return await data1.FirstOrDefaultAsync(x => x.Id == id);
        //}
            
        public bool Remove(T data)
        {
            EntityEntry<T> info = Table.Remove(data);
            return info.State == EntityState.Deleted;
        }

        public async Task<T> RemoveById(int id)
        {
            var data = await Table.FirstOrDefaultAsync(x => x.Id == id);
            Table.Remove(data);
            return data;
        }
        public bool Update(T data)
        {
            EntityEntry<T> info = Table.Update(data);
            return info.State == EntityState.Modified;
        }
    }
}
