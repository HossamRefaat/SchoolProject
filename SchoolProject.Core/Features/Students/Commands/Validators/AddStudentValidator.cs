using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commands.Validators
{
    public class AddStudentValidator : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public AddStudentValidator(IStudentService studentService,
                                    IStringLocalizer<SharedResources> localizer)
        {
            _studentService = studentService;
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationRules()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be a positive integer.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);
        }

        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.NameAr)
                .Custom((command, context) =>
                {
                    // Example custom validation: Ensure that the student's name does not contain numbers
                    if (command != null && System.Text.RegularExpressions.Regex.IsMatch(command, @"\d"))
                    {
                        context.AddFailure("Name", "Student name must not contain numbers.");
                    }
                });

            RuleFor(x => x.NameEn)
                .Custom((command, context) =>
                {
                    // Example custom validation: Ensure that the student's name does not contain numbers
                    if (command != null && System.Text.RegularExpressions.Regex.IsMatch(command, @"\d"))
                    {
                        context.AddFailure("Name", "Student name must not contain numbers.");
                    }
                });

            RuleFor(x => x.NameAr)
                .MustAsync(async (key, CancellationToken) => !await _studentService.IsNameExistsAsync(key))
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.NameEn)
               .MustAsync(async (key, CancellationToken) => !await _studentService.IsNameExistsAsync(key))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion

    }
}
