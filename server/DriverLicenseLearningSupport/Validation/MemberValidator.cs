using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class MemberValidator : AbstractValidator<MemberModel>
    {
        public MemberValidator()
        {
           
            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage("Tên không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage("Tên không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.Phone)
                .Matches("[0-9]{10,12}")
                .WithMessage("Số điện thoại từ 10-12 ký tự");
        }
    }

    public static class MemberValidatorExtension 
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this MemberModel member) 
        {
            var validator = new MemberValidator();
            var result = await validator.ValidateAsync(member);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }

            return null;
        }
    }
}
