using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class ApplicationUserController : AppControllerBase
    {
        [HttpPost(Router.ApplicationUserRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddUserCommand command)
        {
            var res = await Mediator.Send(command);
            return NewResult(res);
        }

        [HttpGet(Router.ApplicationUserRouting.Paginated)]
        public async Task<IActionResult> GetStudenPaginated([FromQuery] GetUserListQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [HttpGet(Router.ApplicationUserRouting.GetUserById)]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var res = await Mediator.Send(new GetUserByIdQuery(id));
            return NewResult(res);
        }

        [HttpPut(Router.ApplicationUserRouting.Edit)]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            var res = await Mediator.Send(command);
            return NewResult(res);
        }

    }
}
