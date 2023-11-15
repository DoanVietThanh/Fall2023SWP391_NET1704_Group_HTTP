using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class AddressValidator : AbstractValidator<AddressModel>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street)
               .NotNull()
               .WithMessage("Đường không chứa ký tự đặc biệt");
            RuleFor(x => x.District)
               .NotNull()
               .WithMessage("Quận không chứa ký tự đặc biệt");
            RuleFor(x => x.City)
               .NotNull()
               .WithMessage("Thành phố không chứa số hoặc ký tự đặc biệt");
        }
    }

    public static class AddressValidatorExtension
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this AddressModel address) 
        {
            var validator = new AddressValidator();
            var result = await validator.ValidateAsync(address);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }

            return null;
        }
    }
}
