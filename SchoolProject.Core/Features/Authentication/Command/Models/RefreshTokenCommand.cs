using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.Helpers;

namespace SchoolProject.Core.Features.Authentication.Command.Models;

public class RefreshTokenCommand : IRequest<Response<JwtAuthRsult>>
{   
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
