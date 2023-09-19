using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IStaffRepository
    {
        Task<bool> CreateAsync(Staff staff);
        Task<StaffModel> FindByIdAsync(Guid id);
        Task<StaffModel> FindByEmailAsync(string email);
    }
}
