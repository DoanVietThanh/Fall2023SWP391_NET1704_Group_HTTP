using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ISlotRepository
    {
        Task<SlotModel> CreateAsync(Slot slot);
        Task<IEnumerable<SlotModel>> GetAllAsync();
    }
}
