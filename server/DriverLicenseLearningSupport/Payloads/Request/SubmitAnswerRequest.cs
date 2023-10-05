using DriverLicenseLearningSupport.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class SubmitAnswerRequest
    {
        [Required(ErrorMessage = "Phải điền email trước khi làm bài")]
        [EmailAddress(ErrorMessage ="sai cú pháp")]
        public string Email { get; set; }
        //public string? MemberId  { get; set; }
        public int TheoryExamId { get; set; }
        public int TotalTime { get; set; } 
        public string StartedDate { get; set; }
        public List<SelectedAnswerModel> SelectedAnswers { get; set; }
        
    }
    public static class SubmitAnswerRequestExtension 
    {
        public static List<ExamGradeModel> ToListExamGradeModel(this SubmitAnswerRequest obj) 
        {
            List<ExamGradeModel> result = new List<ExamGradeModel>();
            foreach (SelectedAnswerModel sa in obj.SelectedAnswers)
            {
                var examGrademodel = new ExamGradeModel
                {
                    //MemberId = obj.MemberId,
                    TheoryExamId = obj.TheoryExamId,
                    Email = obj.Email,
                    QuestionId = sa.QuestionId,
                    SelectedAnswerId = sa.SelectedAnswerId
                };
                result.Add(examGrademodel);
            }
            return result;  
        }
    }
}
