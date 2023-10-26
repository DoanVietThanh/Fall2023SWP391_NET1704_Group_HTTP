using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class RollCallBookRequest
    {
        public string? Comment { get; set; } = null!;
        public bool IsAbsence { get; set; } = true;
        public int? TotalHoursDriven { get; set; } = null!;
        public int? TotalKmDriven { get; set; } = null!;
    }

    public static class RollCallBookRequestExtension
    {
        public static RollCallBookModel ToRollCallBookModel(this RollCallBookRequest reqObj)
        {
            return new RollCallBookModel 
            {
                Comment = reqObj.Comment,
                IsAbsence = reqObj.IsAbsence,
                TotalHoursDriven = reqObj.TotalHoursDriven,
                TotalKmDriven = reqObj.TotalKmDriven
            };
        }
    }
}
