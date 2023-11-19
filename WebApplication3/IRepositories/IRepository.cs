using Microsoft.EntityFrameworkCore;
using WebApplication3.Entities;

namespace WebApplication3.IRepositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        DbSet<T> Table { get; }
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        Task<bool> AddAsync(T data);
        bool Remove(T data);
        Task<T> RemoveById(int id);
        bool Update(T data);
    }
}
