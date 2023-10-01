namespace DriverLicenseLearningSupport.Models
{
    public class AnswerModel
    {
        public int QuestionAnswerId { get; set; }
        public string Answer { get; set; }
        public bool? IsTrue { get; set; }
        public int QuestionId { get; set; }
    }
}
