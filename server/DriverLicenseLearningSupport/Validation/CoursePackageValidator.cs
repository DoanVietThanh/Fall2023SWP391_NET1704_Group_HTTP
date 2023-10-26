using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class CoursePackageValidator : AbstractValidator<CoursePackageModel>
    {
        public CoursePackageValidator()
        {
            RuleFor(x => x.CoursePackageDesc)
                .NotEmpty();
            RuleFor(x => x.Cost)
                .NotEmpty();
            RuleFor(x => x.AgeRequired)
                .NotEmpty();
        }
    }

    public static class CoursePackageValidatorExtension {
        public async static Task<ValidationProblemDetails> ValidateAsync(this CoursePackageModel model)
        {
            var validator = new CoursePackageValidator();
            var result = await validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }
            return null!;
        }
    }
}
