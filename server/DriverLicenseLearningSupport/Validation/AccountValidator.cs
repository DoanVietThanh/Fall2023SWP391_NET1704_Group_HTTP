using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .EmailAddress()
                .WithMessage("Email sai format");
            RuleFor(x => x.Password)
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")
                .WithMessage("Mật khẩu độ dài ít nhất 8 ký tự, chứa ít nhất 1 chữ cái viết hoa và 1 số");
        }
    }
}
