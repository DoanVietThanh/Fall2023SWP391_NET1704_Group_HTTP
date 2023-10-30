using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class SimulationSituationModel
    {
        public int SimulationId { get; set; }
        public string SimulationVideo { get; set; }
        public string ImageResult { get; set; }
        public int? TimeResult { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseTypeModel LicenseType { get; set; }
    }
}
