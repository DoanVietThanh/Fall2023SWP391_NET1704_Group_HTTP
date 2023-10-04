using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class AnswerValidator : AbstractValidator<AnswerModel>
    {
        public AnswerValidator()
        {
            //RuleFor(x => x.Answer)
              //  .Matches("^[a-zA-Z0-9 ]+$")
                //.WithMessage("Nội dung câu trả lời không được có ký tự đặc biệt");
        }

    }
    public static class AnswerValidatorExtension
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this AnswerModel answer)
        {
            var validator = new AnswerValidator();
            var result = await validator.ValidateAsync(answer);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }
            return null;
        }
    }
}
