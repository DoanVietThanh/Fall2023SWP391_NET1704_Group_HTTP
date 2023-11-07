using DriverLicenseLearningSupport.Models;
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
            RuleFor(x => x.IdentityCardImage).SetValidator(new ImageFileValidator());
            RuleFor(x => x.HealthCertificationImage).SetValidator(new ImageFileValidator());
        }
    }

    public class LicenseRegisterFormUpdateValidator : AbstractValidator<LicenseRegisterFormUpdate>
    {
        public LicenseRegisterFormUpdateValidator()
        {
            // validate image file
            RuleFor(x => x.Image).SetValidator(new ImageFileValidator());
            RuleFor(x => x.IdentityCardImage).SetValidator(new ImageFileValidator());
            RuleFor(x => x.HealthCertificationImage).SetValidator(new ImageFileValidator());
        }
    }
}
