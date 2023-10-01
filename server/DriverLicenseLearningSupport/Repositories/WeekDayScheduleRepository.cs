using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Repositories.Impl;

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
    }
}
