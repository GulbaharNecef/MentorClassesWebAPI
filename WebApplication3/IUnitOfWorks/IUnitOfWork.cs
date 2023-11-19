using WebApplication3.Entities;
using WebApplication3.IRepositories;

namespace WebApplication3.IUnitOfWorks
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        Task<int> SaveChangesAsync();
    }
}
