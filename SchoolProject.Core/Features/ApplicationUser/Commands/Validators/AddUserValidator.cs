using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Validators;

public class AddUserValidator : AbstractValidator<AddUserCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    #endregion

    #region Constructor
    public AddUserValidator(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
        ApplyValidationRules();
    }
    #endregion

    #region Methods
    public void ApplyValidationRules()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotFound])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotFound])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])            
            .Equal(x => x.Password).WithMessage(_localizer[SharedResourcesKeys.PasswordNotEqualConfirmPass]);

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

        RuleFor(x => x.Country)
            .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");        
    }
    #endregion
}
