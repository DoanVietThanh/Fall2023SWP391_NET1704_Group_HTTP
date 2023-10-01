using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class WeekDayScheduleService : IWeekDayScheduleService
    {
        private readonly IMapper _mapper;
        private readonly IWeekDayScheduleRepository _weekDayScheduleRepo;

        public WeekDayScheduleService(IWeekDayScheduleRepository weekDayScheduleRepo,
            IMapper mapper)
        {
            _mapper = mapper;
            _weekDayScheduleRepo = weekDayScheduleRepo;
        }

        public async Task<bool> CreateRangeAsync(IEnumerable<WeekdayScheduleModel> weekdays)
        {
            var weekdayEntities = _mapper.Map<IEnumerable<WeekdaySchedule>>(weekdays);
            return await _weekDayScheduleRepo.CreateRangeAsync(weekdayEntities);
        }
    }
}
