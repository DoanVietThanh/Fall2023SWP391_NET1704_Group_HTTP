using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class AccountValidator : AbstractValidator<AccountModel>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Email)
               .EmailAddress()
               .WithMessage("Email sai format");
            RuleFor(x => x.Password)
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")
                .WithMessage("Mật khẩu độ dài ít nhất 8 ký tự, chứa ít nhất 1 chữ cái viết hoa và 1 số");
        }
    }

    public static class AccountValidatorExtension 
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this AccountModel account) 
        {
            var validator = new AccountValidator();
            var result = await validator.ValidateAsync(account);
            if (!result.IsValid) 
            {
                return result.ToProblemDetails();
            }

            return null;
        }
    }
}
