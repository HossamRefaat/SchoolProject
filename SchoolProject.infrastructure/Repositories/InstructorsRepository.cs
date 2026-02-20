using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;

namespace SchoolProject.Infrastructure.Repositories
{
    public class InstructorsRepository : GenericRepositoryAsync<Instructor>, IInstructorsRepository
    {
        #region Fields
        private readonly DbSet<Instructor> _instructors;
        #endregion
        #region Constructor
        public InstructorsRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _instructors = dbContext.Set<Instructor>();
        }
        #endregion
        #region Functions Handling
        #endregion
    }
}
