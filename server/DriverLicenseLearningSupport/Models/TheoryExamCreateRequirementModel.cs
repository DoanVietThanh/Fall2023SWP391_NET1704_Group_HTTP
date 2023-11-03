namespace DriverLicenseLearningSupport.Models
{
    public class TheoryExamCreateRequirementModel
    {
        public int TotalQuestion { get; set; }
        public int TotalAnswerRequired { get; set; }
        public int TotalTime { get; set; }
        public string LicenseTypeDesc { get; set; }
    }
}
