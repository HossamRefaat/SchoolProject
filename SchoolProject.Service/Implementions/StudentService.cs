using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementions
{
    public class StudentService : IStudentService
    {
        #region Fields
        private readonly IStudentRepository _studentRepository;
        #endregion

        #region Constructor
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }


        #endregion

        #region Implemention
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _studentRepository.GetStudentsListAsync();
        }
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            //var student = await _studentRepository.GetByIdAsync(id);
            var student = await _studentRepository.GetTableNoTracking()
                                                  .Include(s => s.Department)
                                                  .Where(s => s.StudID == id)
                                                  .FirstOrDefaultAsync();
            return student;
        }

        public async Task<string> CreateStudentAsync(Student student)
        {
            await _studentRepository.AddAsync(student);
            return "added";
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            var studentExists = _studentRepository.GetTableNoTracking()
                                              .Where(s => s.NameEn.Equals(name)).FirstOrDefault();

            return studentExists != null;
        }

        public async Task<bool> IsNameExistsExecludeSelfAsync(string name, int id)
        {
            var studentExists = await _studentRepository.GetTableNoTracking()
                                              .Where(s => s.NameEn.Equals(name) && !s.StudID.Equals(id)).FirstOrDefaultAsync();
            return studentExists != null;
        }

        public async Task<string> EditStudentAsync(Student student)
        {
            await _studentRepository.UpdateAsync(student);
            return "Success";
        }

        public async Task<string> DeleteStudentAsync(Student student)
        {
            var transaction = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.DeleteAsync(student);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Falid";
            }

        }

        public IQueryable<Student> GetStudentsQueryable()
        {
            return _studentRepository.GetTableNoTracking().Include(s => s.Department).AsQueryable();
        }

        public IQueryable<Student> FilterStudentPaginatedQuerable(StudentOrderingEnum? order, string? search)
        {
            var query = _studentRepository.GetTableNoTracking().Include(s => s.Department).AsQueryable();
            if (search != null)
                query = query.Where(x => x.NameAr.Contains(search) || x.Address.Contains(search));
            switch (order)
            {
                case StudentOrderingEnum.StudId:
                    query = query.OrderBy(s => s.StudID);
                    break;
                case StudentOrderingEnum.Name:
                    query = query.OrderBy(s => s.NameAr);
                    break;
                case StudentOrderingEnum.Address:
                    query = query.OrderBy(s => s.Address);
                    break;
                case StudentOrderingEnum.DepartmentName:
                    query = query.OrderBy(s => s.Department!.DNameAr);
                    break;
                default:
                    query = query.OrderBy(s => s.StudID);
                    break;
            }
            return query;
        }

        public IQueryable<Student> GetStudentByDepartmentIdQuerable(int DID)
        {
            return _studentRepository.GetTableNoTracking().Where(x => x.DID.Equals(DID)).AsQueryable();
        }
        #endregion
    }
}
