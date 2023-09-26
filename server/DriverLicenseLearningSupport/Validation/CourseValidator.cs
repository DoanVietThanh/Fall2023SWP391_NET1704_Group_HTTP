using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class CourseValidator : AbstractValidator<CourseModel>
    {
        public CourseValidator()
        {
            RuleFor(x => x.CourseTitle)
                .NotEmpty();
            RuleFor(x => x.CourseDesc)
                .NotEmpty();
            RuleFor(x => x.Cost)
                .NotEmpty();
        }
    }

    public static class CourseValidatorExtesion 
    {
        public async static Task<ValidationProblemDetails> ValidateAsync(this CourseModel course) 
        {
            var validator = new CourseValidator(); 
            var result = await validator.ValidateAsync(course);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }
            return null;
        }
    }
}
