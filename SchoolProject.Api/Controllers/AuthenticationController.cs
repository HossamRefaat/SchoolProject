using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Authentication.Command.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers;

[ApiController]
public class AuthenticationController : AppControllerBase
{
    
    [HttpPost(Router.Authentication.SignIn)]
    public async Task<IActionResult> Create([FromForm] SignInCommand command)
    {
        var res = await Mediator.Send(command);
            return NewResult(res);
        }
}