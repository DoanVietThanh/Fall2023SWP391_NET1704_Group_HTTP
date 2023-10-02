using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ISlotService
    {
        Task<SlotModel> CreateAsync(SlotModel slot);
        Task<IEnumerable<SlotModel>> GetAllAsync();
    }
}
