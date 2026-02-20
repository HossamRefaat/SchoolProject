using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;

namespace SchoolProject.Infrastructure.Repositories
{
    internal class DepartmentRepository : GenericRepositoryAsync<Department>, IDepartmentRepository
    {
        #region Fields
        private readonly DbSet<Department> departments;

        #endregion

        #region Constructor
        public DepartmentRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            departments = dBContext.Set<Department>();
        }
        #endregion



        #region Handle Functions
        #endregion

        #region
        #endregion

    }
}
