using System.Diagnostics;
using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Features.Authentication.Queries.Models;

public class AuthorizeUserQuerey : IRequest<Response<string>>
{
    public string AccessToken { get; set; }
}
