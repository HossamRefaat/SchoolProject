using SchoolProject.Data.Results;
using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Features.Authorization.Queries.Models;

public class MangeUserClaimsQuery : IRequest<Response<MangeUserClaimsResult>>
{
    public int UserId { get; set; }

}
