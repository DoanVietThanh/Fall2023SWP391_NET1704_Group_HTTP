using DriverLicenseLearningSupport.Models;
using Microsoft.OpenApi.Writers;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class UpdateQuestionRequest
    {
        public int QuestionID { get; set; }
        public string QuestionAnswerDesc { get; set; }
        public bool? IsParalysis { get; set; }
        public string? Image { get; set; }
        public List<AnswerModel> Answers{ get; set; }

    }
    public static class UpdateQuestionRequestExtension {
        public static QuestionModel toQuestionModel(this UpdateQuestionRequest obj) 
        {
            return new QuestionModel()
            {
                QuestionAnswerDesc = obj.QuestionAnswerDesc,
                IsParalysis = obj.IsParalysis,
                Image = obj.Image
            };
        }   
    }
    


}
