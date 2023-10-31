using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public SlotRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SlotModel> CreateAsync(Slot slot)
        {
            await _context.Slots.AddAsync(slot);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (!isSucess) return null;

            return _mapper.Map<SlotModel>(slot);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var slotEntity = await _context.Slots.Where(x => x.SlotId == id)
                                                 .FirstOrDefaultAsync();
            if (slotEntity is null) return false;

            // delete 
            _context.Slots.Remove(slotEntity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<SlotModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SlotModel>>(await _context.Slots.ToListAsync());
        }

        public async Task<SlotModel> GetAsync(int id)
        {
            return _mapper.Map<SlotModel>(await _context.Slots.Where(x => x.SlotId == id).FirstOrDefaultAsync());
        }

        public async Task<bool> UpdateAsync(int id, SlotModel slot)
        {
            var slotEntity = await _context.Slots.Where(x => x.SlotId == id)
                                                 .FirstOrDefaultAsync();

            if (slotEntity == null) return false;

            // update slot features
            slotEntity.SlotDesc = slot.SlotDesc;
            slotEntity.SlotName = slot.SlotName;
            slotEntity.Time = slot.Time;
            slotEntity.Duration = slot.Duration;

            // save changes
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
