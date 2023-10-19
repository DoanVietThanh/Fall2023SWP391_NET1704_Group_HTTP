using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class LicenseRegisterFormValidator : AbstractValidator<LicenseRegisterFormRequest>
    {
        public LicenseRegisterFormValidator()
        {
            // validate image file
            RuleFor(x => x.Image).SetValidator(new ImageFileValidator());
            RuleFor(x => x.IdentityImage).SetValidator(new ImageFileValidator());
            RuleFor(x => x.HealthCertificationImage).SetValidator(new ImageFileValidator());
        }
    }
}
