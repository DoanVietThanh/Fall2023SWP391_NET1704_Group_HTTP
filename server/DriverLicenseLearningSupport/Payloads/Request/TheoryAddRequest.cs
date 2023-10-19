namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class TheoryAddRequest
    {
        public int? TotalQuestion { get; set; }
        public int? TotalTime { get; set; }
        public int? TotalAnswerRequired { get; set; }

        public int[] QuestionIds { get; set; }
    }
}
