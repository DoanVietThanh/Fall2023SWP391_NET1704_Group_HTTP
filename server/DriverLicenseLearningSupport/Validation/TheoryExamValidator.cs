using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class TheoryExamValidator : AbstractValidator<TheoryExamModel>
    {
        public TheoryExamValidator() 
        {
            RuleFor(te => te.TotalQuestion)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please input TotalQuestion");
            RuleFor(te => te.TotalTime)
                .NotNull()
                .NotEmpty()
                .LessThanOrEqualTo(30)
                .WithMessage("Exam Time must be less than 60 minutes");
            RuleFor(te => te.TotalAnswerRequired)
                .NotEmpty()
                .NotNull()
                .WithMessage("Please input TotalAnswerRequired");
        }
    }
}
