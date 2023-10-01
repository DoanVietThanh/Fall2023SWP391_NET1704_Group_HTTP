using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ISlotService
    {
        Task<IEnumerable<SlotModel>> GetAllAsync();
    }
}
