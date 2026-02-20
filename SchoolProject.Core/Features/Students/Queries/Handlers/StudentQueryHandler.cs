using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System.Linq.Expressions;

namespace SchoolProject.Core.Features.Students.Queries.Handlers
{
    public class StudentQueryHandler : ResponseHandler
                                      , IRequestHandler<GetStudentListQuery, Response<List<GetStudentListResponse>>>
                                      , IRequestHandler<GetStudentByIdQuery, Response<GetSingleStudentResponse>>
                                      , IRequestHandler<GetStudentPagenatedListQuery, PaginatedResult<GetStudentListPaginatedResponse>>
    {

        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public StudentQueryHandler(IStudentService studentService,
                                   IMapper mapper,
                                   IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _localizer = localizer;
        }
        #endregion

        #region Function Handling
        public async Task<Response<List<GetStudentListResponse>>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentList = await _studentService.GetStudentsListAsync();
            var studentListMapper = _mapper.Map<List<GetStudentListResponse>>(studentList);
            var res = Success(studentListMapper);
            res.Meta = new
            {
                TotalCount = studentListMapper.Count
            };
            return res;
        }

        public async Task<Response<GetSingleStudentResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentService.GetStudentByIdAsync(request.Id);
            if (student == null)
                return NotFound<GetSingleStudentResponse>(_localizer[SharedResourcesKeys.NotFound]);
            var studentMapper = _mapper.Map<GetSingleStudentResponse>(student);
            return Success(studentMapper);
        }

        public async Task<PaginatedResult<GetStudentListPaginatedResponse>> Handle(GetStudentPagenatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Student, GetStudentListPaginatedResponse>> expression = student => new GetStudentListPaginatedResponse
            {
                StudID = student.StudID,
                Name = student.GetLocal(student.NameAr, student.NameEn),
                Address = student.Address,
                DepartmentName = student.Department != null ? student.GetLocal(student.Department.DNameAr, student.Department.DNameEn) : null
            };
            //var querable = _studentService.GetStudentsQueryable();
            var filterQuery = _studentService.FilterStudentPaginatedQuerable(request.Order, request.Search);
            var studentPaginatedList = await filterQuery
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            studentPaginatedList.Meta = new { Count = studentPaginatedList.Data.Count() };
            return studentPaginatedList;
        }
        #endregion
    }
}
