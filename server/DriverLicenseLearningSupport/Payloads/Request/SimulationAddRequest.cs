using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class SimulationAddRequest
    {
        [Required(ErrorMessage = "Vui lòng truyền video lên")]
        public IFormFile SimulationVideo { get; set; }
        [Required(ErrorMessage ="Vui lòng truyền ảnh đáp án")]
        public IFormFile ImageResult { get; set; }
        public int? TimeResult { get; set; }
        public int LicenseTypeId { get; set; }
    }
    public static class SimulationAddRequestExtend 
    {
        public static SimulationSituationModel ToSimulationSituation(this SimulationAddRequest model, string image, string video) 
        {
            return new SimulationSituationModel
            {
                ImageResult = image,
                SimulationVideo = video,
                TimeResult = model.TimeResult,
                LicenseTypeId = model.LicenseTypeId
            };
        }
    }
}
