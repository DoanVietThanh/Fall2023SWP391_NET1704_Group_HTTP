using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class TheoryExamValidator : AbstractValidator<TheoryExamModel>
    {
        public TheoryExamValidator() 
        {

            RuleFor(te => te.StartTime)
                .NotEmpty()
                .NotNull()
                .WithMessage("Yêu cầu điền thời gian bắt đầu thi");

        }
    }
}
