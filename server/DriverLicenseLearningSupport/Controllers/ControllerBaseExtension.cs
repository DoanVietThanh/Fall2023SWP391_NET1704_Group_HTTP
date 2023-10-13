using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Controllers
{
    public static class ControllerBaseExtension
    {
        public static string? BaseUrl(this ControllerBase controller)
        {
            var req = controller.HttpContext.Request;
            if (req == null) return null;
            var uriBuilder = new UriBuilder(req.Scheme, req.Host.Host, req.Host.Port ?? -1);
            if (uriBuilder.Uri.IsDefaultPort)
            {
                uriBuilder.Port = -1;
            }

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
