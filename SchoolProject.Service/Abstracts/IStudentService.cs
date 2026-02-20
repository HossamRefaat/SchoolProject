using SchoolProject.Data.Entities;
using SchoolProject.Data.Helpers;

namespace SchoolProject.Service.Abstracts
{
    public interface IStudentService
    {
        public Task<List<Student>> GetStudentsListAsync();
        public IQueryable<Student> GetStudentsQueryable();
        public Task<Student> GetStudentByIdAsync(int id);
        public Task<string> CreateStudentAsync(Student student);
        public Task<string> EditStudentAsync(Student student);
        public Task<string> DeleteStudentAsync(Student student);
        public Task<bool> IsNameExistsAsync(string name);
        public Task<bool> IsNameExistsExecludeSelfAsync(string name, int id);
        public IQueryable<Student> FilterStudentPaginatedQuerable(StudentOrderingEnum? order, string? search);
        public IQueryable<Student> GetStudentByDepartmentIdQuerable(int DID);
    }
}
