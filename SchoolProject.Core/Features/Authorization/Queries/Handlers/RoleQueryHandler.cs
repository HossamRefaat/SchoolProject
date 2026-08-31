using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Core.Features.Authorization.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Data.DTOs;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Queries.Handlers;

public class RoleQueryHandler : ResponseHandler,
                                IRequestHandler<GetRolesListQuery, Response<List<GetRolesListResult>>>,
                                IRequestHandler<GetRoleByIdQuery, Response<GetRoleByIdResult>>,
                                IRequestHandler<ManageUserRolesQuery, Response<ManageUserRolesResult>>
{
    #region Fields
    private readonly IAuthorizationService _authorizationService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IMapper _mapper;
    #endregion

    #region constructor
    public RoleQueryHandler(IAuthorizationService authorizationService,
                            IStringLocalizer<SharedResources> localizer,
                            IMapper mapper) : base(localizer)
    {
        _authorizationService = authorizationService;
        _localizer = localizer;
        _mapper = mapper;
    }
    #endregion
    #region Methods
    public async Task<Response<List<GetRolesListResult>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
    {
        var roles = await _authorizationService.GetRolesListAsync();
        var res = _mapper.Map<List<GetRolesListResult>>(roles);
        return Success(res);
    }

    public async Task<Response<GetRoleByIdResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _authorizationService.GetRoleByIdAsync(request.Id);
        if (role == null)
            return NotFound<GetRoleByIdResult>(_localizer[SharedResourcesKeys.RoleNotExist]);
        var res = _mapper.Map<GetRoleByIdResult>(role);
        return Success(res);
    }

    public async Task<Response<ManageUserRolesResult>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
    {
        var res = await _authorizationService.ManageUserRolesAsync(request.UserId);
        if (res == null)
            return NotFound<ManageUserRolesResult>(_localizer[SharedResourcesKeys.NotFound]);
        return Success(res);
    }
    #endregion


}
