using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Security.Certificates;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace DriverLicenseLearningSupport.Repositories
{
    public class TeachingScheduleRepository : ITeachingScheduleRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly CourseSettings _courseSettings;

        public TeachingScheduleRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper,
            IOptionsMonitor<AppSettings> monitor,
            IOptionsMonitor<CourseSettings> courseMonitor)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
            _courseSettings = courseMonitor.CurrentValue;
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
        public async Task<bool> AddCoursePackageAsync(int teachingScheduleId, Guid coursePackageId)
        {
            var teachingSchedule = await _context.TeachingSchedules.Where(x => x.TeachingScheduleId == teachingScheduleId)
                .FirstOrDefaultAsync();

            if(teachingSchedule is not null)
            {
                // add course package
                teachingSchedule.CoursePackageId = coursePackageId.ToString();

                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<TeachingScheduleModel> CreateAsync(TeachingSchedule teachingSchedule)
        {
            await _context.TeachingSchedules.AddAsync(teachingSchedule);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (!isSucess) return null;
            return _mapper.Map<TeachingScheduleModel>(teachingSchedule);
        }
        public async Task<bool> CreateRangeBySlotAndWeekdayAsync(int slotId, string weekdays, int weekdayScheduleId,
            TeachingScheduleModel teachingSchedule)
        {
            // get from config
            var daysInWeek = _courseSettings.WeekdaySchedules;
            // generate list from config
            var listDays = daysInWeek.ToList();
            // not found
            if (!listDays.Contains(weekdays)) return false;
            // get register range days in week
            var rangeDaysRegister = listDays.Where(x => x == weekdays).FirstOrDefault();

            // get weekday schedule by id
            var weekdaySchedule = await _context.WeekdaySchedules.Where(x => x.WeekdayScheduleId == weekdayScheduleId)
                                                                 .FirstOrDefaultAsync();
             
            // get all schedule by course id
            var weekdaySchedules = await _context.WeekdaySchedules.Where(x => x.CourseId == weekdaySchedule.CourseId)
                                                                 .ToListAsync();

            List<DateTime> rangeSchedules 
                = new List<DateTime>();
            if(rangeDaysRegister is "2,4,6")
            {
                foreach(var ws in weekdaySchedules)
                {
                    rangeSchedules.Add(Convert.ToDateTime(ws.Monday));
                    rangeSchedules.Add(Convert.ToDateTime(ws.Wednesday));
                    rangeSchedules.Add(Convert.ToDateTime(ws.Friday));
                }
            }
            else if(rangeDaysRegister is "3,5,7")
            {
                foreach (var ws in weekdaySchedules)
                {
                    rangeSchedules.Add(Convert.ToDateTime(ws.Tuesday));
                    rangeSchedules.Add(Convert.ToDateTime(ws.Thursday));
                    rangeSchedules.Add(Convert.ToDateTime(ws.Saturday));
                }
            }
            else
            {
                foreach (var ws in weekdaySchedules)
                {
                    rangeSchedules.Add(Convert.ToDateTime(ws.Saturday));
                    rangeSchedules.Add(Convert.ToDateTime(ws.Sunday));
                }
            }

            var isSucess = false;
            // generate teaching schedules
            foreach(var dt in rangeSchedules)
            {

                var existSchedule = await GetByMentorIdAndTeachingDateAsync(weekdayScheduleId,
                            Guid.Parse(teachingSchedule.StaffId),
                            dt, slotId);

                if (existSchedule is null)
                {
                    // set teaching schedule details
                    teachingSchedule.WeekdayScheduleId = weekdayScheduleId;
                    teachingSchedule.SlotId = slotId;
                    teachingSchedule.TeachingDate =
                        DateTime.ParseExact(dt.ToString(_appSettings.DateFormat),
                        _appSettings.DateFormat, CultureInfo.InvariantCulture);
                    // teachingSchedule.VehicleId = vehicleId;
                    teachingSchedule.IsActive = false;

                    isSucess = await CreateAsync(_mapper.Map<TeachingSchedule>(teachingSchedule))
                        is not null ? true : false;
                }
            }
            return isSucess;
        }
        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAsync(Guid mentorId)
        {
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
                                                                    .Select(x => new TeachingSchedule
                                                                    {
                                                                        TeachingScheduleId = x.TeachingScheduleId,
                                                                        TeachingDate = x.TeachingDate,
                                                                        IsActive = x.IsActive,
                                                                        WeekdayScheduleId = x.WeekdayScheduleId,
                                                                        SlotId = x.SlotId,
                                                                        VehicleId = x.VehicleId,
                                                                        Slot = x.Slot,
                                                                        Vehicle = x.Vehicle,
                                                                        Staff = x.Staff,
                                                                        StaffId = x.StaffId
                                                                    })
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
                                                                                Member = x.Member,
                                                                                IsAbsence = x.IsAbsence
                                                                        }).ToList(),
                                                                        Staff = x.Staff
                                                                    })
                                                                    .ToListAsync();

            return _mapper.Map<IEnumerable<TeachingScheduleModel>>(teachingSchedules);
        }
        public async Task<TeachingScheduleModel> GetAsync(int teachingScheduleId)
        {
            var teachingSchedule = await _context.TeachingSchedules.Where(x => x.TeachingScheduleId == teachingScheduleId)
                                                                    .Select(x => new TeachingSchedule
                                                                    {
                                                                        TeachingScheduleId = x.TeachingScheduleId,
                                                                        TeachingDate = x.TeachingDate,
                                                                        Vehicle = x.Vehicle,
                                                                        RollCallBooks = x.RollCallBooks.Select(x => new RollCallBook
                                                                        {
                                                                            RollCallBookId = x.RollCallBookId,
                                                                            TeachingScheduleId = x.TeachingScheduleId,
                                                                            IsAbsence = x.IsAbsence,
                                                                            Comment = x.Comment,
                                                                            MemberId = x.MemberId,
                                                                            Member = x.Member,
                                                                            TotalHoursDriven = x.TotalHoursDriven,
                                                                            TotalKmDriven = x.TotalKmDriven
                                                                        }).ToList(),
                                                                        WeekdayScheduleId = x.WeekdayScheduleId,
                                                                        SlotId = x.SlotId,
                                                                        StaffId = x.StaffId,
                                                                        Staff = new Staff
                                                                        {
                                                                            StaffId = x.StaffId,
                                                                            FirstName = x.Staff.FirstName,
                                                                            LastName = x.Staff.LastName,
                                                                            DateBirth = x.Staff.DateBirth,
                                                                            Phone = x.Staff.Phone,
                                                                            Email = x.Staff.Email,
                                                                            AvatarImage = x.Staff.AvatarImage
                                                                        },
                                                                        CoursePackageId = x.CoursePackageId
                                                                    })
                                                                   .FirstOrDefaultAsync();
            return _mapper.Map<TeachingScheduleModel>(teachingSchedule);
        }
        public async Task<TeachingScheduleModel> GetByFilterAsync(TeachingScheduleFilter filters)
        {
            // building query
            var teachingSchedules = _context.TeachingSchedules.AsQueryable();

            //// filters
            //if (!String.IsNullOrEmpty(filters.SlotId.ToString()) && filters.SlotId > 0)
            //{
            //    teachingSchedules = teachingSchedules.Where(x => x.SlotId == filters.SlotId);
            //}
            // filters by weekday schedule id
            //if (!String.IsNullOrEmpty(filters.WeekDayScheduleId.ToString()) && filters.WeekDayScheduleId > 0)
            //{
            //    teachingSchedules = teachingSchedules.Where(x => x.WeekdayScheduleId 
            //                == filters.WeekDayScheduleId);
            //}

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

            //// filters
            //if (!String.IsNullOrEmpty(filters.SlotId.ToString()) && filters.SlotId > 0)
            //{
            //    teachingSchedules = teachingSchedules.Where(x => x.SlotId == filters.SlotId);
            //}
            // filters by weekday schedule id
            //if (!String.IsNullOrEmpty(filters.WeekDayScheduleId.ToString()) && filters.WeekDayScheduleId > 0)
            //{
            //    teachingSchedules = teachingSchedules.Where(x => x.WeekdayScheduleId
            //                == filters.WeekDayScheduleId);
            //}

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
        public async Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(int weekdayScheduleId, Guid mentorId, DateTime date, int slotId)
        {
            var dateFormat = date.ToString("dd/MM/yyyy");
            //var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
            //                                                        .ToListAsync();
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
            .Select(x => new TeachingSchedule
            {
                TeachingScheduleId = x.TeachingScheduleId,
                TeachingDate = x.TeachingDate,
                Vehicle = x.Vehicle,
                RollCallBooks = x.RollCallBooks.Select(x => new RollCallBook { 
                    RollCallBookId = x.RollCallBookId,
                    TeachingScheduleId = x.TeachingScheduleId,
                    IsAbsence = x.IsAbsence,
                    Comment = x.Comment,
                    MemberId = x.MemberId,
                    Member = x.Member
                }).ToList(),
                WeekdayScheduleId = x.WeekdayScheduleId,
                SlotId = x.SlotId,
                StaffId = x.StaffId
            }).ToListAsync();
            var existSchedule = teachingSchedules.Where(x => x.TeachingDate.ToString("dd/MM/yyyy").Equals(dateFormat) 
                                                          && x.SlotId == slotId
                                                          && x.WeekdayScheduleId == weekdayScheduleId)
                                                 .FirstOrDefault();
            return _mapper.Map<TeachingScheduleModel>(existSchedule);
        }
        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByTeachingDateAsync(DateTime date)
        {
            var dateFormat = date.ToString("yyyy-MM-dd");

            var schedules = await _context.TeachingSchedules.ToListAsync();

            schedules = schedules.Where(x => x.TeachingDate.ToString("yyyy-MM-dd")
            .Equals(dateFormat)).ToList();

            return _mapper.Map<IEnumerable<TeachingScheduleModel>>(schedules);
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
                var schedules = await _context.TeachingSchedules.Where(x => x.SlotId == slotId
                                                                        && x.StaffId == mentorId.ToString())
                                                               .Select(x => new TeachingSchedule
                                                               {
                                                                   TeachingScheduleId = x.TeachingScheduleId,
                                                                   TeachingDate = x.TeachingDate,
                                                                   Vehicle = x.Vehicle,
                                                                   CoursePackageId = x.CoursePackageId,
                                                                   CoursePackage = new CoursePackage { 
                                                                        CoursePackageId = x.CoursePackageId,
                                                                        AgeRequired = x.CoursePackage.AgeRequired,
                                                                        Cost = x.CoursePackage.Cost,
                                                                        CoursePackageDesc = x.CoursePackage.CoursePackageDesc,
                                                                        SessionHour = x.CoursePackage.SessionHour,
                                                                        TotalSession = x.CoursePackage.TotalSession
                                                                   },
                                                                   Staff = x.Staff,

                                                                   RollCallBooks = x.RollCallBooks
                                                                    .Select(
                                                                        x => new RollCallBook
                                                                        {
                                                                            RollCallBookId = x.RollCallBookId,
                                                                            MemberId = x.MemberId,
                                                                            Comment = x.Comment,
                                                                            Member = x.Member,
                                                                            TotalHoursDriven = x.TotalHoursDriven,
                                                                            TotalKmDriven = x.TotalKmDriven,
                                                                            IsAbsence = x.IsAbsence,
                                                                            IsActive = x.IsActive,
                                                                            CancelMessage = x.CancelMessage
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
                                                                        IsAbsence = x.IsAbsence,
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
        public async Task<IEnumerable<TeachingScheduleModel>> GetAllAwaitScheduleByMentorAsync(int slotId, int weekDayScheduleId,
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
                var schedules = await _context.TeachingSchedules.Where(x => x.SlotId == slotId
                                                                        && x.StaffId == mentorId.ToString()
                                                                        && x.IsActive == false)
                                                               .Select(x => new TeachingSchedule
                                                               {
                                                                   TeachingScheduleId = x.TeachingScheduleId,
                                                                   TeachingDate = x.TeachingDate,
                                                                   Vehicle = x.Vehicle,
                                                                   IsActive = x.IsActive,
                                                                   CoursePackageId = x.CoursePackageId,
                                                                   CoursePackage = new CoursePackage
                                                                   {
                                                                       CoursePackageId = x.CoursePackageId,
                                                                       AgeRequired = x.CoursePackage.AgeRequired,
                                                                       Cost = x.CoursePackage.Cost,
                                                                       CoursePackageDesc = x.CoursePackage.CoursePackageDesc,
                                                                       SessionHour = x.CoursePackage.SessionHour,
                                                                       TotalSession = x.CoursePackage.TotalSession
                                                                   },
                                                                   RollCallBooks = x.RollCallBooks
                                                                    .Select(
                                                                        x => new RollCallBook
                                                                        {
                                                                            RollCallBookId = x.RollCallBookId,
                                                                            MemberId = x.MemberId,
                                                                            Comment = x.Comment,
                                                                            Member = x.Member,
                                                                            TotalHoursDriven = x.TotalHoursDriven,
                                                                            TotalKmDriven = x.TotalKmDriven,
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
        public async Task<IEnumerable<TeachingScheduleModel>> GetAllAwaitScheduleMentor() 
        {
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.IsActive == false)
                                                                    .Select(x => new TeachingSchedule { 
                                                                        TeachingScheduleId = x.TeachingScheduleId,
                                                                        Staff = x.Staff
                                                                    })
                                                                    .ToListAsync();
            return _mapper.Map<IEnumerable<TeachingScheduleModel>>(teachingSchedules);
        }
        public async Task<TeachingScheduleModel> ExistScheduleInOtherCoursesAsync(int slotId, DateTime teachingDate, Guid mentorId, Guid courseId)
        {
            var weekdays = await _context.WeekdaySchedules.Where(x => x.CourseId != courseId.ToString()).ToListAsync();

            foreach(var wd in weekdays) 
            {
                var teachingSchedules = await _context.TeachingSchedules.Where(x => x.WeekdayScheduleId == wd.WeekdayScheduleId)
                                                                        .ToListAsync();

                var dateFormat = teachingDate.ToString("dd/MM/yyyy");
                var existSchedule = teachingSchedules.Where(x => x.TeachingDate.ToString("dd/MM/yyyy").Equals(dateFormat)
                                                              && x.SlotId == slotId
                                                              && x.StaffId == mentorId.ToString())
                                                     .FirstOrDefault();
                if(existSchedule is not null)
                {
                    return _mapper.Map<TeachingScheduleModel>(existSchedule);
                }
            }

            return null!;
        }

        public async Task<bool> ApproveMentorAwaitSchedule(Guid mentorId)
        {
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
                                                                    .ToListAsync();
            // update schedule status
            foreach(var ts in teachingSchedules)
            {
                ts.IsActive = true;
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeVehicleMentorSchedule(Guid mentorId, int vehicleId)
        {
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
                                                                    .ToListAsync();
            foreach(var ts in teachingSchedules)
            {
                ts.VehicleId = vehicleId;
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<TeachingScheduleModel> GetFirstAwaitScheduleMentor(Guid mentorId)
        {
            var teachingSchedule = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString())
                                                                   .FirstOrDefaultAsync();
            return _mapper.Map<TeachingScheduleModel>(teachingSchedule);
        }

        public async Task<bool> DenyMentorAwaitSchedule(Guid mentorId)
        {
            var teachingSchedules = await _context.TeachingSchedules.Where(x => x.StaffId == mentorId.ToString() 
                                                                             && x.IsActive == false)
                                                                   .ToListAsync();
            // update schedule status
            foreach (var ts in teachingSchedules)
            {
                _context.TeachingSchedules.Remove(ts);
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetAllWeekdayScheduleAsync(int weekDayScheduleId)
        {
            var schedules = await _context.TeachingSchedules.Where(x => x.WeekdayScheduleId == weekDayScheduleId)
                                                            .ToListAsync();
            return _mapper.Map<IEnumerable<TeachingScheduleModel>>(schedules);
        }
    }
}
