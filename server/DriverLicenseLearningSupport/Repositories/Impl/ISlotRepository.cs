using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ISlotRepository
    {
        Task<SlotModel> CreateAsync(Slot slot);
        Task<SlotModel> GetAsync(int id);
        Task<IEnumerable<SlotModel>> GetAllAsync();
        Task<bool> UpdateAsync(int id, SlotModel slot);
        Task<bool> DeleteAsync(int id);
    }
}
