using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class VehicleValidator : AbstractValidator<VehicleModel>
    {
        public VehicleValidator()
        {
            RuleFor(x => x.VehicleName)
                .Matches("^[a-zA-Z0-9 ]+$")
                .WithMessage("Tên xe không ký tự đặc biệt");
            RuleFor(x => x.VehicleLicensePlate)
                .Matches(@"^[0-9]{2}[a-zA-Z]{1}-[0-9]{3}.[0-9]{2}$")
                .WithMessage("Vui lòng chỉ nhập số, chữ cái, hoặc kí tự chấm/gạch ngang");
        }
    }

    public static class VehicleValidatorExtesion
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this VehicleModel vehicle)
        {
            var validator = new VehicleValidator();
            var result = await validator.ValidateAsync(vehicle);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }

            return null;
        }
    }
}
