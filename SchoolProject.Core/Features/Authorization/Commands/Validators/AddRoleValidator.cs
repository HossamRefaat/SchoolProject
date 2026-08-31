using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Validators;

public class AddRoleValidator : AbstractValidator<AddRoleCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IAuthorizationService _authorizationService;
    #endregion

    #region Constructor
    public AddRoleValidator(IStringLocalizer<SharedResources> localizer,
                            IAuthorizationService authorizationService)
    {
        _localizer = localizer;
        _authorizationService = authorizationService;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }
    
    private void ApplyValidationRules()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
    }

    private void ApplyCustomValidationRules()
    {
        RuleFor(x => x.RoleName)
            .MustAsync(async (key, cancellationToken) => !await _authorizationService.IsRoleExist(key))
            .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
    }
    #endregion
}
