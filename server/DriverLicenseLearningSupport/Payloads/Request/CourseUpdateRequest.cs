using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DriverLicenseLearningSupport.Payloads.Request
{ 
    public class CourseUpdateRequest
    {
        [Required(ErrorMessage = "Course Title is required")]
        public string CourseTitle { get; set; } = null!;

        [Required(ErrorMessage = "Course Description is required")]
        public string CourseDesc { get; set; } = null!;

        [Required(ErrorMessage = "Total Hours is required")]
        public int? TotalHourRequired { get; set; }
        [Required(ErrorMessage = "Total Km is required")]
        public int? TotalKmRequired { get; set; }
    }

    public static class CourseUpdateRequestExtension
    {
        public static CourseModel ToCourseModel(this CourseUpdateRequest reqObj)
        {
            return new CourseModel()
            {
                CourseTitle = reqObj.CourseTitle,
                CourseDesc = WebUtility.HtmlEncode(reqObj.CourseDesc),
                TotalHoursRequired = reqObj.TotalHourRequired,
                TotalKmRequired = reqObj.TotalKmRequired,
                IsActive = true
            };
        }
    }
}
