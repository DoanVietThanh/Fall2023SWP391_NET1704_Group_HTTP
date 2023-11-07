using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IAccountService
    {
        Task<AccountModel> CheckLoginAsync(string username, string password);
        Task<bool> CreateAsync(AccountModel account);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
        Task<AccountModel> GetByEmailAsync(string email);
        Task<IEnumerable<AccountModel>> GetAllAsync();
        Task<bool> DeleteAsync(string email);
        Task<bool> BanAccountAsync(string email);
        Task<bool> UnBanAccountAsync(string email);
    }
}