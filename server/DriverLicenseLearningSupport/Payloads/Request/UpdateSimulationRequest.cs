using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class UpdateSimulationRequest
    {
        public int ID { get; set; }
        public IFormFile? ImageResult { get; set; }
        public int? TimeResult { get; set; }

    }
    public static class UpdateSimulationRequestExtension
    {
        public static SimulationSituationModel ToSimulationSituationModel(this UpdateSimulationRequest obj)
        {
            return new SimulationSituationModel()
            {
                TimeResult = obj.TimeResult
            };
        }
    }
}
