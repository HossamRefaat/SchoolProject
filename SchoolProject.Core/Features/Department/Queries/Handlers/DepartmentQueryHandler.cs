using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Department.Queries.Models;
using SchoolProject.Core.Features.Department.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System.Linq.Expressions;

namespace SchoolProject.Core.Features.Department.Queries.Handlers
{
    public class DepartmentQueryHandler : ResponseHandler,
        IRequestHandler<GetDepartmentByIdQuery, Response<GetDepartmentByIdResponse>>
    {
        #region Fields
        private readonly IDepartmentService departmentService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IMapper mapper;
        private readonly IStudentService studentService;
        #endregion

        #region Constructors
        public DepartmentQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
            IDepartmentService departmentService,
            IMapper mapper,
            IStudentService studentService) : base(stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            this.departmentService = departmentService;
            this.mapper = mapper;
            this.studentService = studentService;
        }
        #endregion

        #region Handle Function 
        public async Task<Response<GetDepartmentByIdResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await departmentService.GetDepartmentByIdAsync(request.Id);
            if (response == null) return NotFound<GetDepartmentByIdResponse>(stringLocalizer[SharedResourcesKeys.NotFound]);
            var mapping = mapper.Map<GetDepartmentByIdResponse>(response);
            Expression<Func<Student, StudentResponse>> expression = e => new StudentResponse(e.StudID, e.GetLocal(e.NameAr, e.NameEn));
            var studentQuerable = studentService.GetStudentByDepartmentIdQuerable(request.Id);
            var paginatedList = await studentQuerable.Select(expression).ToPaginatedListAsync(request.StudentPageNumber, request.StudentPageSize);
            mapping.StudentList = paginatedList;

            return Success(mapping);
        }
        #endregion

    }
}
