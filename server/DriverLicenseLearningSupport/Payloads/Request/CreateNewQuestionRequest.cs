using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CreateNewQuestionRequest
    {
        // Question
        [Required(ErrorMessage = "Please input Question's detail")]
        public string questionAnswerDesc { get; set; } = null;
        //public string? ImageLink { get; set; } = null;
        public IFormFile? imageLink { get; set; } = null!;

        //Answer
        [Required(ErrorMessage = "Please input Answer's detail")]
        public string[] answers { get; set; } = null; //dap a, dap b..
        
        public string rightAnswer { get; set; } = null!;
        

        public bool? isParalysis { get; set; } = null;

        //License Type for the question
        [Required(ErrorMessage ="Please input LisenceTypeID")]
        public int LicenseTypeId { get; set; }

        //create question, contxt.Questio.OrderByDesc.FirstOrD
    }

    public static class CreateNewQuestionRequestExtend
    {

        public static QuestionModel ToQuestionModel(this CreateNewQuestionRequest obj)
        {
            return new QuestionModel
            {
                QuestionAnswerDesc = obj.questionAnswerDesc, //question detail
                IsParalysis = String.Compare(obj.isParalysis.ToString(), "True") == 0 ? true : false,
                LicenseTypeId = obj.LicenseTypeId
            };
        }
        public static List<AnswerModel> ToListAnswerModel(this CreateNewQuestionRequest obj)
        {
            List<AnswerModel> result = new List<AnswerModel>();
            foreach (var str in obj.answers)
            {
                bool isTrue = false;
                if (str.Equals(obj.rightAnswer))
                {
                    isTrue = true;
                }
                var answerModel = new AnswerModel
                {
                    Answer = str,
                    IsTrue = isTrue
                };
                result.Add(answerModel);
                //answer.IsTrue = String.Compare(answer.IsTrue.ToString(), "True") == 0 ? true : false;
                //result.Add(answer);
            }
            return result;
        }
        
    }

}
