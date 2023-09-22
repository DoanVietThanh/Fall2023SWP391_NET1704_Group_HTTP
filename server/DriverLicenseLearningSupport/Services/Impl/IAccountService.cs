using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IAccountService
    {
        Task<AccountModel> CheckLoginAsync(string username, string password);
        Task<bool> CreateAsync(AccountModel account);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
        Task<AccountModel> GetByEmailAsync(string email);
        Task<bool> DeleteAsync(string email);
    }
}
