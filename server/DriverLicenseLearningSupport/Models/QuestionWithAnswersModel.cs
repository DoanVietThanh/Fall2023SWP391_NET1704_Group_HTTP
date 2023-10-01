namespace DriverLicenseLearningSupport.Models
{
    public class QuestionWithAnswersModel
    {
        public QuestionModel question { get; set; }

        public IEnumerable<AnswerModel> answers { get; set; }
    }
}
