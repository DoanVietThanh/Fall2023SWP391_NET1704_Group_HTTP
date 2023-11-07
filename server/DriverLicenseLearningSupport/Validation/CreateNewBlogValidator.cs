using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class CreateNewBlogValidator : AbstractValidator<BlogCreateRequest>
    {
        public CreateNewBlogValidator() 
        {
            RuleFor(x => x.Image).SetValidator(new ImageFileValidator());
        }
    }
}
