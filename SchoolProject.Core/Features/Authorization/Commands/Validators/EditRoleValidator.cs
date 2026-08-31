using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Validators;

public class EditRoleValidator: AbstractValidator<EditRoleCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    #endregion

    #region Constructor
    public EditRoleValidator(IStringLocalizer<SharedResources> localizer
                             )
    {
        _localizer = localizer;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }
    
    private void ApplyValidationRules()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
    }

    private void ApplyCustomValidationRules()
    {

    }
    #endregion
}