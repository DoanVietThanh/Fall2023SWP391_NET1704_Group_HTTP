namespace DriverLicenseLearningSupport.VnPay.Interface
{
    public interface ICurrentUserService
    {
        string? UserID { get; }
        string? IpAddress { get; }
    }
}
