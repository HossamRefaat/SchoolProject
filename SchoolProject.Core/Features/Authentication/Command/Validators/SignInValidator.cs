using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authentication.Command.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.Authentication.Command.Validators;

public class SignInValidator : AbstractValidator<SignInCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    #endregion

    #region Constructor
    public SignInValidator(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
        ApplyValidationRules();
    }
    #endregion  

    #region Methods
    public void ApplyValidationRules()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
    }
    #endregion
}
