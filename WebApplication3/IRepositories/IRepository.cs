using Microsoft.EntityFrameworkCore;
using WebApplication3.Entities;

namespace WebApplication3.IRepositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        DbSet<T> Table { get; }//bu o demekdir ki Table adinda bir deyisken tanimlayiram ve onun tipi DbSet<T> dir, yeni o bir database tablesi dir bir class ve ya basqa bir sey deyil
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        Task<bool> AddAsync(T data);
        bool Remove(T data);
        Task<T> RemoveById(int id);
        bool Update(T data);
    }
}
