using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IStaffService
    {
        Task<bool> CreateAsync(StaffModel model);
        Task<StaffModel> FindByEmailAsync(string email);
        Task<StaffModel> FindByIdAsync(Guid id);
    }
}
