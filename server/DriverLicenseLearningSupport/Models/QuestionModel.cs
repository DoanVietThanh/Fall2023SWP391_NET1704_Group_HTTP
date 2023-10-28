using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class QuestionModel
    {
        
        public int QuestionId { get; set; }
        public string QuestionAnswerDesc { get; set; }
        public bool? IsParalysis { get; set; }
        public string? Image { get; set; }
        public bool? IsActive { get; set; }
        public int LicenseTypeId { get; set; }


        public virtual LicenseTypeModel LicenseType { get; set; }

        public virtual ICollection<AnswerModel> QuestionAnswers { get; set; } = new List<AnswerModel>();  
    }
}
