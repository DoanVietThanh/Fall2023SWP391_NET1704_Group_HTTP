using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Utils;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class TeachingScheduleRepository : ITeachingScheduleRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public TeachingScheduleRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TeachingScheduleModel> CreateAsync(TeachingSchedule teachingSchedule)
        {
            await _context.TeachingSchedules.AddAsync(teachingSchedule);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (!isSucess) return null;
            return _mapper.Map<TeachingScheduleModel>(teachingSchedule);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorId(Guid mentorId)
        {
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
                                                                    .ToListAsync();
            return _mapper.Map<IEnumerable<TeachingScheduleModel>>(teachingSchedules);
        }

        public async Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(Guid mentorId, DateTime date)
        {
            var dateFormat = date.ToString("dd/MM");
            //var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
            //                                                        .ToListAsync();
            var teachingSchedules = await _context.TeachingSchedules.ToListAsync();
            var existSchedule = teachingSchedules.Where(x => x.TeachingDate.ToString("dd/MM").Equals(dateFormat))
                                                 .FirstOrDefault();
            return _mapper.Map<TeachingScheduleModel>(existSchedule);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleAsync(int slotId, int weekDayScheduleId,
            Guid mentorId)
        {
            var weekday = await _context.WeekdaySchedules.Where(x => x.WeekdayScheduleId == weekDayScheduleId)
                                                         .FirstOrDefaultAsync();

            var weekdayModel = _mapper.Map<WeekdayScheduleModel>(weekday);
            var dates = DateTimeHelper.GenerateDateTimesFromWeekDay(weekdayModel);

            var teachingSchedules = new List<TeachingScheduleModel>();
            foreach (var d in dates)
            {
                var dateFormat = d.ToString("dd/MM/yyyy");
                var schedules = await _context.TeachingSchedules.Where(x => x.SlotId == slotId && x.StaffId == mentorId.ToString())
                                                               .ToListAsync();

                // filter date
                var schedule = schedules.Where(x => x.TeachingDate.ToString("dd/MM/yyyy").Equals(
                    dateFormat)).FirstOrDefault();

                if (schedule is not null)
                {
                    teachingSchedules.Add(_mapper.Map<TeachingScheduleModel>(schedule));    
                }
                else
                {
                    teachingSchedules.Add(null);
                }
            }
            return teachingSchedules;
        }
    }
}
