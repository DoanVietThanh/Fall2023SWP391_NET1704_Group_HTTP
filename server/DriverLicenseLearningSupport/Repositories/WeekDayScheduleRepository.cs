using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

        public async Task<IEnumerable<WeekdayScheduleModel>> GetAllByCourseId(Guid courseId)
        {
            var weekdays = await _context.WeekdaySchedules.Where(x => x.CourseId == courseId.ToString())
                                                    .ToListAsync();
            return _mapper.Map<IEnumerable<WeekdayScheduleModel>>(weekdays);
        }

        public async Task<WeekdayScheduleModel> GetAsync(int id)
        {
            // get weekday by id
            var weekdayEntity = await _context.WeekdaySchedules.Where(x => x.WeekdayScheduleId == id)
                                                               .FirstOrDefaultAsync();
            return _mapper.Map<WeekdayScheduleModel>(weekdayEntity);
        }

        public async Task<WeekdayScheduleModel> GetByDateAndCourseId(DateTime date, Guid courseId)
        {
            var findDate = await _context.WeekdaySchedules.Where(x => date >= x.Monday
                                                            && date <= x.Sunday 
                                                            && x.CourseId == courseId.ToString())
                                                          .FirstOrDefaultAsync();
            if (findDate is not null)
            {
                var weekday = await _context.WeekdaySchedules.Where(x => x.WeekdayScheduleId == findDate.WeekdayScheduleId)
                                                         .FirstOrDefaultAsync();

                if (weekday is not null) return _mapper.Map<WeekdayScheduleModel>(weekday);
            }
            return null;
        }

        public async Task<WeekdayScheduleModel> GetByDateAsync(DateTime date)
        {
            var findDate = await _context.WeekdaySchedules.Where(x => date >= x.Monday
                                                            && date <= x.Sunday)
                                                          .FirstOrDefaultAsync();
            if(findDate is not null)
            {
                var weekday = await _context.WeekdaySchedules.Where(x => x.WeekdayScheduleId == findDate.WeekdayScheduleId)
                                                         .FirstOrDefaultAsync();

                if (weekday is not null) return _mapper.Map<WeekdayScheduleModel>(weekday);
            }
            return null;
        }
    }
}
