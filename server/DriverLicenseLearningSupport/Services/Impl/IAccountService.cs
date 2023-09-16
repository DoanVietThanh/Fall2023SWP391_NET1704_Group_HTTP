using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.impl
{
    public interface IAccountService
    {
        Task<AccountModel> CheckLoginAsync(string username, string password);
        Task<bool> CreateAsync(AccountModel account);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
        Task<AccountModel> FindByEmailAsync(string email);
    }
}
