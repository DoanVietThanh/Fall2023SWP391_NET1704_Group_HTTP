using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace DriverLicenseLearningSupport.Repositories
{
    public class TeachingScheduleRepository : ITeachingScheduleRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public TeachingScheduleRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper,
            IOptionsMonitor<AppSettings> monitor)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
        }

        public async Task<bool> AddRollCallBookAsync(int teachingScheduleId, RollCallBook rcbModel)
        {
            // get teachign schedule by id
            var teachingSchedule = await _context.TeachingSchedules.Where(x => x.TeachingScheduleId == teachingScheduleId)
                                                                   .FirstOrDefaultAsync();
            // not found schedule
            if (teachingSchedule is null) return false;

            // add roll call book
            teachingSchedule.RollCallBooks.Add(rcbModel);
            // save changes
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> AddVehicleAsync(int teachingScheduleId, int vehicleId)
        {
            // get teachign schedule by id
            var teachingSchedule = await _context.TeachingSchedules.Where(x => x.TeachingScheduleId == teachingScheduleId)
                                                                   .FirstOrDefaultAsync();
            // not found schedule
            if (teachingSchedule is null) return false;

            // set vehicle 
            teachingSchedule.VehicleId = vehicleId;
            // save changes
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<TeachingScheduleModel> CreateAsync(TeachingSchedule teachingSchedule)
        {
            await _context.TeachingSchedules.AddAsync(teachingSchedule);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (!isSucess) return null;
            return _mapper.Map<TeachingScheduleModel>(teachingSchedule);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAsync(Guid mentorId)
        {
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
                                                                    .ToListAsync();
            return _mapper.Map<IEnumerable<TeachingScheduleModel>>(teachingSchedules);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAndMemberIdAsync(Guid mentorId, Guid memberId)
        {
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
                                                                    .Select(x => new TeachingSchedule { 
                                                                        TeachingScheduleId = x.TeachingScheduleId,
                                                                        TeachingDate = x.TeachingDate,
                                                                        Slot = x.Slot,
                                                                        Vehicle = x.Vehicle,
                                                                        RollCallBooks = x.RollCallBooks
                                                                        .Where(x => x.MemberId == memberId.ToString())
                                                                        .Select(
                                                                            x => new RollCallBook { 
                                                                                RollCallBookId = x.RollCallBookId,
                                                                                MemberId = x.MemberId,
                                                                                Comment = x.Comment,
                                                                                Member = x.Member
                                                                        }).ToList(),
                                                                        Staff = x.Staff
                                                                    })
                                                                    .ToListAsync();

            return _mapper.Map<IEnumerable<TeachingScheduleModel>>(teachingSchedules);
        }

        public async Task<TeachingScheduleModel> GetAsync(int teachingScheduleId)
        {
            var teachingSchedule = await _context.TeachingSchedules.Where(x => x.TeachingScheduleId == teachingScheduleId)
                                                                   .FirstOrDefaultAsync();
            return _mapper.Map<TeachingScheduleModel>(teachingSchedule);
        }

        public async Task<TeachingScheduleModel> GetByFilterAsync(TeachingScheduleFilter filters)
        {
            // building query
            var teachingSchedules = _context.TeachingSchedules.AsQueryable();

            // filters
            if (!String.IsNullOrEmpty(filters.SlotId.ToString()) && filters.SlotId > 0)
            {
                teachingSchedules = teachingSchedules.Where(x => x.SlotId == filters.SlotId);
            }
            // filters by weekday schedule id
            if (!String.IsNullOrEmpty(filters.WeekDayScheduleId.ToString()) && filters.WeekDayScheduleId > 0)
            {
                teachingSchedules = teachingSchedules.Where(x => x.WeekdayScheduleId 
                            == filters.WeekDayScheduleId);
            }

            // filters by null teaching date 
            if (filters.TeachingDate.Equals(new DateTime(1, 1, 0001)))
            {
                // get current datetime
                filters.TeachingDate = DateTime.Now;
            }

            // get list teaching schedule
            var result = await teachingSchedules.ToListAsync();

            // format datetime
            var dateFormat = Convert.ToDateTime(filters.TeachingDate).ToString("yyyy-MM-dd");
            // get all teaching schedule have same teaching date
            var resultSchedule = result.Where(x =>
                    x.TeachingDate.ToString("yyyy-MM-dd").Equals(dateFormat)).FirstOrDefault();

            return _mapper.Map<TeachingScheduleModel>(resultSchedule);
        }

        public async Task<TeachingScheduleModel> GetMemberScheduleByFilterAsync(LearningScheduleFilter filters, Guid memberId)
        {
            // building query
            var teachingSchedules = _context.TeachingSchedules.AsQueryable()
                .Select(x => new TeachingSchedule
                {
                    WeekdayScheduleId = x.WeekdayScheduleId,
                    TeachingScheduleId = x.TeachingScheduleId,
                    TeachingDate = x.TeachingDate,
                    Slot = x.Slot,
                    Vehicle = x.Vehicle,
                    RollCallBooks = x.RollCallBooks
                        .Where(x => x.MemberId == memberId.ToString())
                        .Select(
                            x => new RollCallBook
                            {
                                RollCallBookId = x.RollCallBookId,
                                MemberId = x.MemberId,
                                Comment = x.Comment,
                                Member = x.Member
                            }).ToList(),
                    Staff = x.Staff
                });

            // filters
            if (!String.IsNullOrEmpty(filters.SlotId.ToString()) && filters.SlotId > 0)
            {
                teachingSchedules = teachingSchedules.Where(x => x.SlotId == filters.SlotId);
            }
            // filters by weekday schedule id
            if (!String.IsNullOrEmpty(filters.WeekDayScheduleId.ToString()) && filters.WeekDayScheduleId > 0)
            {
                teachingSchedules = teachingSchedules.Where(x => x.WeekdayScheduleId
                            == filters.WeekDayScheduleId);
            }

            // filters by null teaching date 
            if (filters.LearningDate.Equals(new DateTime(1, 1, 0001)))
            {
                // get current datetime
                filters.LearningDate = DateTime.Now;
            }

            // get list teaching schedule
            var result = await teachingSchedules.ToListAsync();


            // format datetime
            var dateFormat = Convert.ToDateTime(filters.LearningDate).ToString("yyyy-MM-dd");
            // get all teaching schedule have same teaching date
            var resultSchedule = result.Where(x =>
                    x.TeachingDate.ToString("yyyy-MM-dd").Equals(dateFormat)).FirstOrDefault();

            return _mapper.Map<TeachingScheduleModel>(resultSchedule);
        }
        public async Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(Guid mentorId, DateTime date, int slotId)
        {
            var dateFormat = date.ToString("dd/MM/yyyy");
            //var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
            //                                                        .ToListAsync();
            var teachingSchedules = await _context.TeachingSchedules.ToListAsync();
            var existSchedule = teachingSchedules.Where(x => x.TeachingDate.ToString("dd/MM/yyyy").Equals(dateFormat) 
                                                          && x.SlotId == slotId)
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

        public async Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleOfMemberAsync(int slotId, int weekDayScheduleId,
            Guid mentorId, Guid memberId)
        {
            var weekday = await _context.WeekdaySchedules.Where(x => x.WeekdayScheduleId == weekDayScheduleId)
                                                         .FirstOrDefaultAsync();

            var weekdayModel = _mapper.Map<WeekdayScheduleModel>(weekday);
            var dates = DateTimeHelper.GenerateDateTimesFromWeekDay(weekdayModel);

            var teachingSchedules = new List<TeachingScheduleModel>();
            foreach (var d in dates)
            {
                var dateFormat = d.ToString("dd/MM/yyyy");
                // get all by slot, staff, member
                var schedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString() 
                                                                         && x.SlotId == slotId)
                                                            .Select(x => new TeachingSchedule
                                                            {
                                                                TeachingScheduleId = x.TeachingScheduleId,
                                                                TeachingDate = x.TeachingDate,
                                                                Vehicle = x.Vehicle,
                                                                RollCallBooks = x.RollCallBooks
                                                                .Where(x => x.MemberId == memberId.ToString())
                                                                .Select(
                                                                    x => new RollCallBook
                                                                    {
                                                                        RollCallBookId = x.RollCallBookId,
                                                                        MemberId = x.MemberId,
                                                                        Comment = x.Comment,
                                                                        Member = x.Member,
                                                                        IsAbsence = x.IsAbsence
                                                                    }).ToList()
                                                            })
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
