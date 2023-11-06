using DriverLicenseLearningSupport.Models;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class CommentValidator : AbstractValidator<CommentModel>
    {
        public CommentValidator() 
        {
            RuleFor(c => c.Content.Trim())
                .NotEmpty()
                .NotNull()
                .WithMessage("Nội dung bình luận không được trống");
        }  
    }
}
