using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Results;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Queries.Handlers;

public class ClaimsQueryHandler : ResponseHandler,
                                  IRequestHandler<MangeUserClaimsQuery, Response<MangeUserClaimsResult>>
{
    #region Fields
    private readonly IAuthorizationService _authorizationService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    #endregion

    #region Constructor
    public ClaimsQueryHandler(IAuthorizationService authorizationService,
                              IStringLocalizer<SharedResources> localizer) : base(localizer)
    {
        _authorizationService = authorizationService;
        _localizer = localizer;
    }
    #endregion

    #region Methods
    public async Task<Response<MangeUserClaimsResult>> Handle(MangeUserClaimsQuery request, CancellationToken cancellationToken)
    {
        var res = await _authorizationService.ManageUserClaimsAsync(request.UserId);
        if (res == null)
            return NotFound<MangeUserClaimsResult>(_localizer[SharedResourcesKeys.NotFound]);

        return Success(res);    
    }
    #endregion
}
