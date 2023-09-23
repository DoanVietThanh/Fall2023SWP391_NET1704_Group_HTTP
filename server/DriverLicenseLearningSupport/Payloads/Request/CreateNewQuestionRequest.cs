using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CreateNewQuestionRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string[] Answer { get; set; }


    }
}
