using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IAccountRepository
    {
        Task<bool> CreateAsync(Account account);
        Task<AccountModel> FindByUsernameAndPasswordAsync(string username, string password);
        Task<AccountModel> FindByEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
    }
}
