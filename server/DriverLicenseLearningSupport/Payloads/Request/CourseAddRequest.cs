using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CourseAddRequest
    {
        [Required(ErrorMessage = "Course Title is required")]
        public string CourseTitle { get; set; } = null!;

        [Required(ErrorMessage = "Course Description is required")]
        public string CourseDesc { get; set; } = null!;

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "License Type is required")]
        public int LicenseTypeId { get; set; }

        [Required(ErrorMessage = "Total hours is required")]
        public int TotalHoursRequired { get; set; }
        
        [Required(ErrorMessage = "License Type is required")]
        public int TotalKmRequired { get; set; }
        [Required(ErrorMessage = "Total Month is required")]
        public int TotalMonth { get; set; }
    }

    public static class CourseAddRequestExtension 
    {
        public static CourseModel ToCourseModel(this CourseAddRequest reqObj)
        {
            return new CourseModel()
            {
                CourseId = Guid.NewGuid().ToString(),
                CourseTitle = reqObj.CourseTitle,
                CourseDesc = WebUtility.HtmlEncode(reqObj.CourseDesc),
                TotalMonth = reqObj.TotalMonth,
                TotalHoursRequired = reqObj.TotalHoursRequired,
                TotalKmRequired = reqObj.TotalKmRequired,
                StartDate = reqObj.StartDate,
                LicenseTypeId = reqObj.LicenseTypeId,
                IsActive = false
            };
        }
    }
}
