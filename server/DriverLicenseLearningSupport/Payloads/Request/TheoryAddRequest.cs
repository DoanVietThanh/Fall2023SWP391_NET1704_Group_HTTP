namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class TheoryAddRequest
    {
        public int? TotalQuestion { get; set; }
        public int? TotalTime { get; set; }
        public int? TotalAnswerRequired { get; set; }
        public bool? IsMockTest { get; set; }
        public string StartDate { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }

        public int[] QuestionIds { get; set; }
    }
}
