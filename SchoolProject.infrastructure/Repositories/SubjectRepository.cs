using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;

namespace SchoolProject.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepositoryAsync<Subject>, ISubjectRepository
    {
        #region Fields
        private readonly DbSet<Subject> _subject;
        #endregion
        #region Constructor
        public SubjectRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _subject = dbContext.Set<Subject>();
        }
        #endregion
        #region Functions Handling
        #endregion
    }
}
