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
               .Matches("^[a-zA-Z ]+$")
               .WithMessage("Đường không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.District)
               .Matches("^[a-zA-Z ]+$")
               .WithMessage("Quận không chứa số hoặc ký tự đặc biệt");
            RuleFor(x => x.City)
               .Matches("^[a-zA-Z ]+$")
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
