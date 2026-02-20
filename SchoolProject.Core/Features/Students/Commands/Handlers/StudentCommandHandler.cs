using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler,
                                         IRequestHandler<AddStudentCommand, Response<string>>,
                                         IRequestHandler<EditStudenCommand, Response<string>>,
                                         IRequestHandler<DeleteStudentCommand, Response<string>>

    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public StudentCommandHandler(IStudentService studentService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _localizer = localizer;
        }
        #endregion

        #region Handling Functions
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var studemntMapper = _mapper.Map<Student>(request);
            var result = await _studentService.CreateStudentAsync(studemntMapper);
            if (result == "added") return Created<string>("");
            else return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditStudenCommand request, CancellationToken cancellationToken)
        {
            var isStudentExist = await _studentService.GetStudentByIdAsync(request.Id);
            if (isStudentExist == null) return NotFound<string>("");
            var student = _mapper.Map(request, isStudentExist);
            var res = await _studentService.EditStudentAsync(student);
            if (res == "Success") return Success((string)_localizer[SharedResourcesKeys.Updated]);
            else return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var isStudentExist = await _studentService.GetStudentByIdAsync(request.Id);
            if (isStudentExist == null) return NotFound<string>();
            var res = await _studentService.DeleteStudentAsync(isStudentExist);
            if (res == "Success") return Deleted<string>();
            else return BadRequest<string>();
        }
        #endregion

    }
}
