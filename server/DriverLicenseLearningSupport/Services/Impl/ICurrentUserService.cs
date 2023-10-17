namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICurrentUserService
    {
        string? UserID { get; }
        string? IpAddress { get; }
    }
}
