using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _slotRepo;
        private readonly IMapper _mapper;

        public SlotService(ISlotRepository slotRepo,
            IMapper mapper)
        {
            _slotRepo = slotRepo;
            _mapper = mapper;
        }

        public async Task<SlotModel> CreateAsync(SlotModel slot)
        {
            var slotEntity = _mapper.Map<Slot>(slot);
            return await _slotRepo.CreateAsync(slotEntity);
        }

        public async Task<IEnumerable<SlotModel>> GetAllAsync()
        {
            return await _slotRepo.GetAllAsync();
        }
    }
}
