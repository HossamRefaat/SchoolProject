using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void GetStudentListMapping()
        {
            CreateMap<Student, GetStudentListResponse>()
              .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.GetLocal(src.Department.DNameAr, src.Department.DNameEn)))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocal(src.NameAr, src.NameEn)));
        }
    }
}
