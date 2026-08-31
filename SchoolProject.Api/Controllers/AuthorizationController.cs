using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
public class AuthorizationController : AppControllerBase
{
    
    [HttpPost(Router.Authorization.Create)]
    public async Task<IActionResult> Create([FromForm] AddRoleCommand command)
    {
        var res = await Mediator.Send(command);
        return NewResult(res);
    }

    [HttpGet(Router.Authorization.GetRolesList)]
    public async Task<IActionResult> GetRolesList()
    {
        var res = await Mediator.Send(new GetRolesListQuery());
        return NewResult(res);
    }

    [HttpGet(Router.Authorization.GetRoleById)]
    public async Task<IActionResult> GetRoleById([FromRoute] int id)
    {
        var res = await Mediator.Send(new GetRoleByIdQuery { Id = id });
        return NewResult(res);
    }

    [HttpPut(Router.Authorization.Edit)]
    public async Task<IActionResult> Edit([FromForm] EditRoleCommand command)
    {
        var res = await Mediator.Send(command);
        return NewResult(res);
    }

    [HttpDelete(Router.Authorization.Delete)]
    public async Task<IActionResult> Delete([FromForm] DeleteRoleCommand command)
    {
        var res = await Mediator.Send(command);
        return NewResult(res);
    }

    [HttpGet(Router.Authorization.ManageUserRoles)]
    public async Task<IActionResult> ManageUserRoles([FromRoute] int userId)
    {
        var res = await Mediator.Send(new ManageUserRolesQuery { UserId = userId });
        return NewResult(res);
    }

    [HttpPut(Router.Authorization.UpdateUserRoles)]
    public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
    {
        var res = await Mediator.Send(command);
        return NewResult(res);
    }

    [HttpGet(Router.Authorization.ManageUserClaims)]
    public async Task<IActionResult> ManageUserClaims([FromRoute] int userId)
    {
        var res = await Mediator.Send(new MangeUserClaimsQuery { UserId = userId });
        return NewResult(res);
    }

    [HttpPut(Router.Authorization.UpdateUserClaims)]
    public async Task<IActionResult> UpdateUserClaims([FromBody] UpdateUserClaimsCommand command)
    {
        var res = await Mediator.Send(command);
        return NewResult(res);
    }
}