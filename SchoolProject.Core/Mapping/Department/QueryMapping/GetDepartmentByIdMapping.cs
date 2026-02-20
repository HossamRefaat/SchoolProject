using AutoMapper;
using SchoolProject.Core.Features.Department.Queries.Results;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.Departments
{
    public partial class DepartmentProfile
    {
        public void GetDepartmentByIdMapping()
        {
            // Map Department -> GetDepartmentByIdResponse
            CreateMap<Department, GetDepartmentByIdResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocal(src.DNameAr, src.DNameEn)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DID))
                .ForMember(dest => dest.MangerName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.GetLocal(src.Instructor.ENameAr, src.Instructor.ENameEn) : string.Empty))
                .ForMember(dest => dest.SubjectList, opt => opt.MapFrom(src => src.DepartmentSubjects))
                .ForMember(dest => dest.StudentList, opt => opt.MapFrom(src => src.Students))
                .ForMember(dest => dest.InstructorList, opt => opt.MapFrom(src => src.Instructors));

            // Map collection of Student -> PaginatedResult<StudentResponse>
            CreateMap<ICollection<Student>, PaginatedResult<StudentResponse>>()
                .ConvertUsing((src, dest, context) =>
                {
                    var students = context.Mapper.Map<List<StudentResponse>>(src);
                    return PaginatedResult<StudentResponse>.Success(
                        students,
                        students.Count,
                        page: 1,
                        pageSize: students.Count == 0 ? 1 : students.Count);
                });

            CreateMap<DepartmentSubject, SubjectResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubID))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Subject.GetLocal(src.Subject.SubjectNameAr, src.Subject.SubjectNameEn)));

            CreateMap<Student, StudentResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudID))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocal(src.NameAr, src.NameEn)));


            CreateMap<Instructor, InstructorResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsId))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocal(src.ENameAr, src.ENameEn)));
        }
    }
}
