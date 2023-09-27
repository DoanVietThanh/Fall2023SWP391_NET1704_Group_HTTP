namespace DriverLicenseLearningSupport.Models.Config
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public int PageSize { get; set; }
        public int TimeOut { get; set; }
        public string DateFormat { get; set; }
        public string DefaultAvatar { get; set; }
        public string MembersCacheKey { get; set; }
        public string StaffsCacheKey { get; set; }
    }
}
