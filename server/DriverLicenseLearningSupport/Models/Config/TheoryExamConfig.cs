namespace DriverLicenseLearningSupport.Models.Config
{
    public class TheoryExamConfig
    {
        public TheoryExamConfig()
        {
            CreateRules = new List<TheoryExamCreateRequirementModel>();
        }
        public List<TheoryExamCreateRequirementModel> CreateRules { get; set; }

    }
}
