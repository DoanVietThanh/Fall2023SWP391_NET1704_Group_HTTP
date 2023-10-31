using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.OpenApi.Writers;

namespace DriverLicenseLearningSupport.Validation
{
    public class BlogValidator :AbstractValidator<BlogModel>
    {
        public BlogValidator() 
        {
            RuleFor(x => x.Content).NotEmpty();
        }
    }
    public static class BlogValidatiorExtension 
    {
        public static async Task<HttpValidationProblemDetails> ValidateAsync(this BlogModel blog) 
        {
            var validator = new BlogValidator();
            var result = await validator.ValidateAsync(blog);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }

            return null;
        }
    }
}
