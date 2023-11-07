using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class StaffValidator : AbstractValidator<StaffModel>
    {
        public StaffValidator()
        {
            RuleFor(x => x.FirstName)
              .NotNull()
              .WithMessage("Họ không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.LastName)
              .NotNull()
              .WithMessage("Tên không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.Phone)
                .Matches("[0-9]{10,12}")
                .WithMessage("Số điện thoại từ 10-12 ký tự");
        }
    }

    public static class StaffValidatorExtension 
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this StaffModel staff) 
        {
            var validator = new StaffValidator();
            var result = await validator.ValidateAsync(staff);
            if(!result.IsValid)
            {
                return result.ToProblemDetails();
            }
            return null;
        }
    }
}
