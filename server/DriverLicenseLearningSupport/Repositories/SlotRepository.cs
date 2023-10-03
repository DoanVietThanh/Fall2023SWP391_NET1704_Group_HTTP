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

        public async Task<IEnumerable<SlotModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SlotModel>>(await _context.Slots.ToListAsync());
        }
    }
}
