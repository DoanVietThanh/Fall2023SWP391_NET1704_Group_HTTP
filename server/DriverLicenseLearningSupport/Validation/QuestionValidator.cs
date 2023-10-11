using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class QuestionValidator : AbstractValidator<QuestionModel>
    {
        public QuestionValidator()
        {
            //RuleFor(x => x.QuestionAnswerDesc)
                //.Matches("^[a-zA-Z0-9 ]+$")
                //.WithMessage("Nội dung câu hỏi hoặc ký tự đặc biệt");
        }
    }
    public static class QuestionValidatorExtension 
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this QuestionModel question) 
        {
            var validator = new QuestionValidator();
            var result = await validator.ValidateAsync(question);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }

            return null;
        }
    }
}
