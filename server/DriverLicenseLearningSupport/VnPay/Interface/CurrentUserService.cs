using System.Security.Claims;

namespace DriverLicenseLearningSupport.VnPay.Interface
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string? UserID => httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? IpAddress => httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();
    }
}
