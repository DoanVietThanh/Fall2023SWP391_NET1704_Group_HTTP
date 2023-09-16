using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("First name not contain number or special character");
            RuleFor(x => x.LastName)
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("Last name not contain number or special character");
            RuleFor(x => x.Username)
                .EmailAddress()
                .WithMessage("Wrong email format");
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Please input email");
            RuleFor(x => x.Password)
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")
                .WithMessage("Password must 8 length, contains at least one uppercase letter" +
                ", one digit");
            RuleFor(x => x.Phone)
                .Matches("[0-9]{10,12}")
                .WithMessage("Phone number must contains 10-12 number");
        }
    }
}
