using FluentValidation;
using System;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserVMValidator : AbstractValidator<UserVM>
    {
        public UserVMValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .WithName("User Name");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .WithName("First Name");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .WithName("Last Name");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("A valid {PropertyName} is required.")
                .WithName("Email");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("{PropertyName} must be valid and contain only digits.")
                .WithName("Phone Number");

            RuleFor(x => x.Dob)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .LessThan(DateTime.Now).WithMessage("{PropertyName} must be in the past.")
                .GreaterThan(DateTime.Now.AddYears(-150)).WithMessage("Age must not exceed 150 years.")
                .WithName("Date of Birth");
        }
    }
}
