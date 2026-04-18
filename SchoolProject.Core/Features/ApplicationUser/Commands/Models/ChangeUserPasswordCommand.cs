using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Models;

public class ChangeUserPasswordCommand : IRequest<Response<string>>
{
    public int Id { get; set; }
    public string CurrnetPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
