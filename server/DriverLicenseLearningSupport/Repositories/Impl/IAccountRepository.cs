using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IAccountRepository
    {
        Task<bool> CreateAsync(Account account);
        Task<AccountModel> GetByUsernameAndPasswordAsync(string username, string password);
        Task<AccountModel> GetByEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
        Task<bool> DeleteAsync(string email);
    }
}