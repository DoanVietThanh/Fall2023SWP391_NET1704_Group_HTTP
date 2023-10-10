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

        public async Task<IEnumerable<WeekdayScheduleModel>> GetAllAsync()
        {
            return await _weekDayScheduleRepo.GetAllAsync();
        }

        public async Task<IEnumerable<WeekdayScheduleModel>> GetAllByCourseId(Guid courseId)
        {
            return await _weekDayScheduleRepo.GetAllByCourseId(courseId);
        }

        public async Task<WeekdayScheduleModel> GetAsync(int id)
        {
            return await _weekDayScheduleRepo.GetAsync(id);
        }

        public async Task<WeekdayScheduleModel> GetByDateAndCourseId(DateTime date, Guid courseId)
        {
            return await _weekDayScheduleRepo.GetByDateAndCourseId(date, courseId);
        }

        public async Task<WeekdayScheduleModel> GetByDateAsync(DateTime date)
        {
            return await _weekDayScheduleRepo.GetByDateAsync(date);
        }
    }
}
