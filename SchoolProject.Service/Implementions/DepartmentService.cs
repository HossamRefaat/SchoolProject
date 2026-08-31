using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementions
{
    internal class DepartmentService : IDepartmentService
    {
        #region Fields
        private readonly IDepartmentRepository departmentRepository;
        #endregion

        #region Constructor
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            var department = await departmentRepository.GetTableNoTracking().Where(x => x.DID.Equals(id))
                 .Include(x => x.DepartmentSubjects).ThenInclude(x => x.Subject)
                 //.Include(x => x.Students)
                 .Include(x => x.Instructors)
                 .Include(x => x.Instructor).FirstOrDefaultAsync();

            return department;
        }

        public async Task<bool> IsDepartmentExist(int id)
        {
            return await departmentRepository.GetTableNoTracking().AnyAsync(x => x.DID.Equals(id));
        }
        #endregion

    }
}
