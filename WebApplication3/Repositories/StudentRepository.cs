using WebApplication3.Entities;
using WebApplication3.IRepositories.IStudentRepos;

namespace WebApplication3.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(NewDbContext db) : base(db)
        {

        }
    }
}
