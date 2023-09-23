using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class ExcelFileValidator : AbstractValidator<IFormFile>
    {
        public ExcelFileValidator()
        {
            RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                                                         || x.Equals("application/vnd.ms-excel"))
                .WithMessage("File type '.xlsx / .xlsm / .xlsb / .xlsx' are required");
        }
    }
}
