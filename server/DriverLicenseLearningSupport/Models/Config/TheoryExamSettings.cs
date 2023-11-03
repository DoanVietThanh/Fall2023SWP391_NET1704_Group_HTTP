namespace DriverLicenseLearningSupport.Models.Config
{
    public class TheoryExamSettings
    {
        public TheoryExamSettings()
        {
            CreateRules = new List<TheoryExamCreateRequirementModel>();
            TotalSubmitTheory = new List<TheoryExamTotalSubmitModel>();
        }
        public List<TheoryExamCreateRequirementModel> CreateRules { get; set; }
        public List<TheoryExamTotalSubmitModel> TotalSubmitTheory { get; set; }   
    }
}
