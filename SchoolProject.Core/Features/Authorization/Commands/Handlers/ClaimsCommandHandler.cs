using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Handlers;

public class ClaimsCommandHandler : ResponseHandler, IRequestHandler<UpdateUserClaimsCommand, Response<string>>
{
    #region Fields
    IStringLocalizer<SharedResources> _localizer;
    IAuthorizationService _authorizationService;

    #endregion

    #region Constructors
    public ClaimsCommandHandler(IStringLocalizer<SharedResources> localizer,
                                IAuthorizationService authorizationService) : base(localizer)
    {
        _localizer = localizer;
        _authorizationService = authorizationService;
    }
    #endregion

    #region Methods
    public  async Task<Response<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
    {
        var res = await _authorizationService.UpdateUserClaimsAsync(request);
        return res switch
        {
            "Success" => Success<string>(_localizer[SharedResourcesKeys.Updated]),
            "notFound" => NotFound<string>(_localizer[SharedResourcesKeys.NotFound]),
            "FailedToRemoveClaims" => BadRequest<string>(_localizer[SharedResourcesKeys.FailedToRemoveOldClaims]),
            "FailedToAddClaims" => BadRequest<string>(_localizer[SharedResourcesKeys.FailedToAddNewClaims]),
            _ => BadRequest<string>(res)
        };
    }
    #endregion
}
