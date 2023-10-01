using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ISlotRepository
    {
        Task<IEnumerable<SlotModel>> GetAllAsync();
    }
}
