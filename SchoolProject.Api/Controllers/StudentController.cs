using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class StudentController : AppControllerBase
    {


        [HttpGet(Router.StudentRouting.GetStudentList)]
        public async Task<IActionResult> GetStudentList()
        {
            var res = await Mediator.Send(new GetStudentListQuery());
            return NewResult(res);
        }

        [HttpGet(Router.StudentRouting.GetStudentPaginated)]
        public async Task<IActionResult> GetStudenPaginated([FromQuery] GetStudentPagenatedListQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [HttpGet(Router.StudentRouting.GetStudentById)]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            var res = await Mediator.Send(new GetStudentByIdQuery(id));
            return NewResult(res);
        }

        [HttpPost(Router.StudentRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddStudentCommand command)
        {
            var res = await Mediator.Send(command);
            return NewResult(res);
        }

        [HttpPut(Router.StudentRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditStudenCommand command)
        {
            var res = await Mediator.Send(command);
            return NewResult(res);
        }

        [HttpDelete(Router.StudentRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var res = await Mediator.Send(new DeleteStudentCommand(id));
            return NewResult(res);
        }
    }
}
