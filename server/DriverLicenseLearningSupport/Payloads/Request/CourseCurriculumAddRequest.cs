using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CourseCurriculumAddRequest
    {
        // CourseId
        [Required(ErrorMessage = "Course Id is required")]
        public Guid CourseId { get; set; }
        // Curriculum_Desc
        [Required(ErrorMessage = "Curriculum description is required")]
        public string CurriculumDesc { get; set; }
        // Curriculum_Detail
        [Required(ErrorMessage = "Curriculum detail is required")]
        public string CurriculumDetail { get; set; }
    }

    public static class CourseCurriculumAddRequestExtension
    {
        public static CurriculumModel ToCurriculumModel(this CourseCurriculumAddRequest reqObj) 
        {
            return new CurriculumModel
            {
                CurriculumDesc = WebUtility.HtmlEncode(reqObj.CurriculumDesc),
                CurriculumDetail = WebUtility.HtmlEncode(reqObj.CurriculumDetail)
            };
        }
    }
}
