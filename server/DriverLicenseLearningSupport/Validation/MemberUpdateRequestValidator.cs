using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class MemberUpdateRequestValidator : AbstractValidator<MemberUpdateRequest>
    {
        public MemberUpdateRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("Họ không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.LastName)
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("Tên không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.Street)
               .Matches("^[a-zA-Z ]+$")
               .WithMessage("Đường không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.District)
               .Matches("^[a-zA-Z ]+$")
               .WithMessage("Quận không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.City)
               .Matches("^[a-zA-Z ]+$")
               .WithMessage("Thành phố không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.Phone)
                .Matches("^[0-9]{10,12}$")
                .WithMessage("Số điện thoại chỉ từ 10-12 ký tự");
        }
    }
}
