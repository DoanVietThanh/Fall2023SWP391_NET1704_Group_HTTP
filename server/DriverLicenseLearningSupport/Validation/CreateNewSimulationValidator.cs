using DriverLicenseLearningSupport.Payloads.Request;
using FluentValidation;

namespace DriverLicenseLearningSupport.Validation
{
    public class CreateNewSimulationValidator : AbstractValidator<SimulationAddRequest>
    {
        public CreateNewSimulationValidator() 
        {
            RuleFor(x => x.SimulationVideo).SetValidator(new ImageFileValidator());
            RuleFor(x => x.ImageResult).SetValidator(new ImageFileValidator());
        }
    }
}
