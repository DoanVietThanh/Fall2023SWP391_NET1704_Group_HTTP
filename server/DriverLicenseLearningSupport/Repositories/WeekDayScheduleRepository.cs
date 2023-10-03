using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class WeekDayScheduleRepository : IWeekDayScheduleRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public WeekDayScheduleRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> CreateRangeAsync(IEnumerable<WeekdaySchedule> weekdays)
        {
            await _context.WeekdaySchedules.AddRangeAsync(weekdays);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<WeekdayScheduleModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<WeekdayScheduleModel>>(
                await _context.WeekdaySchedules.ToListAsync());
        }

        public async Task<WeekdayScheduleModel> GetByDateAsync(DateTime date)
        {
            var findDate = await _context.WeekdaySchedules.Where(x => x.Monday <= date 
                                                            && x.Sunday >= date)
                                                          .FirstOrDefaultAsync();

            var weekday = await _context.WeekdaySchedules.Where(x => x.WeekdayScheduleId == findDate.WeekdayScheduleId)
                                                         .FirstOrDefaultAsync();

            if(weekday is not null) return _mapper.Map<WeekdayScheduleModel>(weekday);
            return null;
        }
    }
}
