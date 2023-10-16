using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CoursePackageUpdateRequest
    {
        public string CoursePackageDesc { get; set; }
        public int? TotalSession { get; set; } = 0;
        public int? SessionHour { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Nhập giá sai, vui lòng nhập lại")]
        public double Cost { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Nhập tuổi sai, vui lòng nhập lại")]
        public int AgeRequired { get; set; }
    }

    public static class CoursePackageUpdateRequestExtension
    {
        public static CoursePackageModel ToCoursePackageModel(
            this CoursePackageUpdateRequest reqObj)
        {
            return new CoursePackageModel{
                CoursePackageDesc = reqObj.CoursePackageDesc,
                TotalSession = reqObj.TotalSession,
                SessionHour = reqObj.SessionHour,
                Cost = reqObj.Cost,
                AgeRequired = reqObj.AgeRequired
            };
        }
    }
}
