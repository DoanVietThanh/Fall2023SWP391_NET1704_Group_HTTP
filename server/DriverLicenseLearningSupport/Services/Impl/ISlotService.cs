using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ISlotService
    {
        Task<SlotModel> CreateAsync(SlotModel slot);
        Task<SlotModel> GetAsync(int id);
        Task<IEnumerable<SlotModel>> GetAllAsync();
        Task<bool> UpdateAsync(int id,  SlotModel slot);
        Task<bool> DeleteAsync(int id);
    }
}
