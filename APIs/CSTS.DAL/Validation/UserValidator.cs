using FluentValidation;
using CSTS.DAL.Models;
using CSTS.DAL.Enum;

namespace CSTS.DAL.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User name is required.")
                .Length(3, 50).WithMessage("User name must be between 3 and 50 characters.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Please Enter your Name.")
                .Length(3, 50).WithMessage("Full Name must be between 3 and 50 characters.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid mobile number.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.RegistrationDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Registration date must be in the past or present.");
        }
    }
}
