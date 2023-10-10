namespace DriverLicenseLearningSupport.Models.Momo.Config
{
    public class MomoConfig
    {
        public static string ConfigName => "Momo";
        public static string PartnerCode { get; set; } = string.Empty;
        public static string ReturnURl { get; set; } = string.Empty;
        public static string IpnURL { get; set; } = string.Empty;
        public static string AccessKey { get; set; } = string.Empty;
        public static string SecretKey { get; set; } = string.Empty;
        public static string PaymenUrl { get; set; } = string.Empty;
    }
}
