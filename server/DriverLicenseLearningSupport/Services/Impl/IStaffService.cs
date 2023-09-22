using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IStaffService
    {
        Task<bool> CreateAsync(StaffModel model);
        Task<StaffModel> GetByEmailAsync(string email);
        Task<StaffModel> GetAsync(Guid id);
    }
}
