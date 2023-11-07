using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class UpdateBlogValidator: AbstractValidator<UpdateBlogRequest>
    {
        public UpdateBlogValidator() 
        {
            RuleFor(x => x.Image).SetValidator(new ImageFileValidator());
        }
    }
}
