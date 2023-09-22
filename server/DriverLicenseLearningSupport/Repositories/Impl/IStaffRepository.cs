using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IStaffRepository
    {
        Task<bool> CreateAsync(Staff staff);
        Task<StaffModel> GetAsync(Guid id);
        Task<StaffModel> GetByEmailAsync(string email);
    }
}
