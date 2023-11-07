namespace DriverLicenseLearningSupport.Models.Config
{
    public class AppSettingsConfig
    {
        public string AllowedHosts { get; set; } = "*";
        public ConnectionStrings ConnectionStrings { get; set; }
        //public Logging Logging { get; set; }
        public AppSettings AppSettings { get; set; }
        public CourseSettings CourseSettings { get; set; }
        public TheoryExamSettings TheoryExamSettings { get; set; }
        public EmailConfiguration EmailConfiguration { get; set; }
        public EPPlus EPPlus { get; set; }
        public VnPayConfig VnPayConfig { get; set; }
    }
}
