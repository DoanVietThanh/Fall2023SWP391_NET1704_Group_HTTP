using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IStaffService
    {
        Task<bool> CreateAsync(StaffModel model);
        Task<StaffModel> GetByEmailAsync(string email);
        Task<StaffModel> GetAsync(Guid id);
        Task<StaffModel> GetMentorAsync(Guid id);
        Task<IEnumerable<StaffModel>> GetAllMentorAsync();
        Task<IEnumerable<StaffModel>> GetAllAsync();
        Task<IEnumerable<StaffModel>> GetAllByFilterAsync(StaffFilter filters);
        Task<bool> UpdateAsync(Guid id, StaffModel staff);
        Task<bool> DeleteAsync(Guid id);
    }
}
