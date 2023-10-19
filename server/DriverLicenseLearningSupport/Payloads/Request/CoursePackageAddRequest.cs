using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CoursePackageAddRequest 
    {
        public string CoursePackageDesc { get; set; }
        public int? TotalSession { get; set; }
        public int? SessionHour { get; set;}
        public double Cost { get; set; }
        public int AgeRequired { get; set; }
    }

    public static class CoursePackageAddRequestExtension
    {
        public static CoursePackageModel ToCoursePackageModel(this CoursePackageAddRequest reqObj)
        {
            return new CoursePackageModel { 
                CoursePackageDesc = reqObj.CoursePackageDesc,
                Cost = reqObj.Cost,
                AgeRequired = reqObj.AgeRequired,
                TotalSession = reqObj.TotalSession,
                SessionHour = reqObj.SessionHour
            };
        }
    }
}
