using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _slotRepo;

        public SlotService(ISlotRepository slotRepo)
        {
            _slotRepo = slotRepo;
        }
        public async Task<IEnumerable<SlotModel>> GetAllAsync()
        {
            return await _slotRepo.GetAllAsync();
        }
    }
}
