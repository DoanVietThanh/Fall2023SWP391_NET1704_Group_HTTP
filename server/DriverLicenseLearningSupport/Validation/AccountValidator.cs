using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.username)
                .EmailAddress()
                .WithMessage("Wrong email format");
            RuleFor(x => x.username)
                .NotEmpty()
                .WithMessage("Please input email"); 
            RuleFor(x => x.password)
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")
                .WithMessage("Password must 8 length, contains at least one uppercase letter" +
                ", one digit");
        }
    }
}
