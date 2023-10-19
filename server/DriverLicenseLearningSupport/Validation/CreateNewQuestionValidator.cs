using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class CreateNewQuestionValidator : AbstractValidator<NewQuestionAddRequest>
    {
        public CreateNewQuestionValidator() 
        {
            RuleFor(x => x.imageLink).SetValidator(new ImageFileValidator());   
        }
    }
}
