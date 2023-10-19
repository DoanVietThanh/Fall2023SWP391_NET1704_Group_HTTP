using DriverLicenseLearningSupport.Models;
using MimeKit;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage message);
        MimeMessage CreateEmailMessage(EmailMessage message);
        void Send(MimeMessage mailMessage);
    }
}
