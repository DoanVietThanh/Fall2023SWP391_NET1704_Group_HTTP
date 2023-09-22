using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .EmailAddress()
                .WithMessage("Vui lòng nhập lại email!");
            RuleFor(x => x.FirstName)
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("Họ không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.LastName)
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("Tên không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.Phone)
                .Matches("[0-9]{10,12}")
                .WithMessage("Số điện thoại từ 10-12 ký tự");
            RuleFor(x => x.Password)
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")
                .WithMessage("Mật khẩu độ dài ít nhất 8 ký tự, chứa ít nhất 1 chữ cái viết hoa và 1 số");
        }
    }
}
