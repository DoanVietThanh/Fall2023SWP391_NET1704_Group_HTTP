using DriverLicenseLearningSupport.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Validation
{
    public class SimulationValidator : AbstractValidator<SimulationSituationModel>
    {
        public SimulationValidator()
        {
            RuleFor(x => x.ImageResult).NotNull();
            RuleFor(x => x.SimulationVideo).NotNull();
        }
    }
    public static class SimulationValidatorExtension
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this SimulationSituationModel simulation)
        {
            var validator = new SimulationValidator();
            var result = await validator.ValidateAsync(simulation);
            if (!result.IsValid)
            {
                return result.ToProblemDetails();
            }
            return null;
        }
    }
}
