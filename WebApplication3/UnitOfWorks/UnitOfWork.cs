using WebApplication3.Entities;
using WebApplication3.IRepositories;
using WebApplication3.IUnitOfWorks;
using WebApplication3.Repositories;

namespace WebApplication3.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewDbContext _db;
        private  Dictionary<Type, object> _repositories;
        public UnitOfWork(NewDbContext db)
        {
            _db = db;
            _repositories = new Dictionary<Type, object>();
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }
            IRepository<TEntity> _repository = new Repository<TEntity>(_db);
            _repositories.Add(typeof(TEntity), _repository);
            return _repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
