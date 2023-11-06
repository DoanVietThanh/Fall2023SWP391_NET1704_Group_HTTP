using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class CreateNewCommentValidator : AbstractValidator<CreateNewCommentRequest>
    {
        //public CreateNewCommentValidator() 
        //{
        //    RuleFor(x => x.AvatarImage).SetValidator(new ImageFileValidator());
        //}
    }
}
