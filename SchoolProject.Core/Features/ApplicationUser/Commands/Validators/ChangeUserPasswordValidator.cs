using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Validators;

public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    #endregion

    #region Constructor
    public ChangeUserPasswordValidator(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
    }
    #endregion

    #region Methods
    public void ApplyValidationRules()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage(_localizer["UserIdGreaterThanZero"]);
            
        RuleFor(x => x.CurrnetPassword)
            .NotEmpty().WithMessage(_localizer["CurrentPasswordRequired"]);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(_localizer["NewPasswordRequired"])
            .MinimumLength(6).WithMessage(_localizer["NewPasswordMinLength"]);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(_localizer["ConfirmPasswordRequired"])
            .Equal(x => x.NewPassword).WithMessage(_localizer["PasswordsDoNotMatch"]);
    }   
    #endregion
}
