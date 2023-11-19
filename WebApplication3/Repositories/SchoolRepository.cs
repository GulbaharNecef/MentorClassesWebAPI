using WebApplication3.Entities;
using WebApplication3.IRepositories.ISchoolRepos;

namespace WebApplication3.Repositories
{
    public class SchoolRepository : Repository<School>, ISchoolRepository
    {
        public SchoolRepository(NewDbContext db) : base(db)
        {

        }
    }
}
