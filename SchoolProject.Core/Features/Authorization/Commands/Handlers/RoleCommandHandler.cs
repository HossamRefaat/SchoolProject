using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Handlers;

public class RoleCommandHandler : ResponseHandler,
                                  IRequestHandler<AddRoleCommand, Response<string>>,
                                  IRequestHandler<EditRoleCommand, Response<string>>,
                                  IRequestHandler<DeleteRoleCommand, Response<string>>,
                                  IRequestHandler<UpdateUserRolesCommand, Response<string>>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IAuthorizationService _authorizationService;
    #endregion

    #region Constructor
    public RoleCommandHandler(IStringLocalizer<SharedResources> localizer,
                              IAuthorizationService authorizationService) : base(localizer)
    {
        _localizer = localizer;
        _authorizationService = authorizationService;
    }
    #endregion

    #region Methods
    public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var  res = await _authorizationService.AddRoleAsync(request.RoleName);
        if(res == "Success") return Success("");
        return BadRequest<string>(_localizer[SharedResourcesKeys.AddFailed]);
    }

    public async Task<Response<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        var res = await _authorizationService.EditRoleAsync(request.Id, request.Name);
        if(res == "Success") return Success<string>(_localizer[SharedResourcesKeys.Updated]);
        if(res == "notFound") return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
        return BadRequest<string>(res);
    }

    public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var res = await _authorizationService.DeleteRoleAsync(request.Id);
        if(res == "Success") return Success<string>(_localizer[SharedResourcesKeys.Deleted]);
        if(res == "Used") return BadRequest<string>(_localizer[SharedResourcesKeys.RoleIsUsed]);
        if(res == "notFound") return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
        return BadRequest<string>(res);
    }

    public async Task<Response<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var res = await _authorizationService.UpdateUserRolesAsync(request);
        if(res == "Success") return Success<string>(_localizer[SharedResourcesKeys.Updated]);
        return BadRequest<string>(res);
    }
    #endregion
}
