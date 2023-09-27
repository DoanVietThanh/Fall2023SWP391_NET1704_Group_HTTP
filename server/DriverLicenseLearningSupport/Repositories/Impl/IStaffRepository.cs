using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IStaffRepository
    {
        Task<bool> CreateAsync(Staff staff);
        Task<StaffModel> GetAsync(Guid id);
        Task<StaffModel> GetMentorAsync(Guid id);
        Task<StaffModel> GetByEmailAsync(string email);
        Task<IEnumerable<StaffModel>> GetAllAsync();
        Task<IEnumerable<StaffModel>> GetAllMentorAsync();
        Task<IEnumerable<StaffModel>> GetAllByFilterAsync(StaffFilter filters);
        Task<bool> UpdateAsync(Guid id, Staff staff);
        Task<bool> DeleteAsync(Guid id);
    }
}
