using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.Helpers;

namespace SchoolProject.Core.Features.Authentication.Command.Models;

public class SignInCommand : IRequest<Response<JwtAuthRsult>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
