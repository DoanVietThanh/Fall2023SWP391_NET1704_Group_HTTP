using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;
using System.Runtime.InteropServices;

namespace DriverLicenseLearningSupport.Services
{
    public class TeachingScheduleService : ITeachingScheduleService
    {
        private ITeachingScheduleRepository _teachingScheduleRepo;
        private IMapper _mapper;

        public TeachingScheduleService(ITeachingScheduleRepository teachingScheduleRepo,
            IMapper mapper)
        {
            _teachingScheduleRepo = teachingScheduleRepo;
            _mapper = mapper;
        }

        public async Task<bool> AddRollCallBookAsync(int teachingScheduleId, RollCallBookModel rcbModel)
        {
            var rcbEntity = _mapper.Map<RollCallBook>(rcbModel);
            return await _teachingScheduleRepo.AddRollCallBookAsync(teachingScheduleId, rcbEntity);
        }

        public async Task<bool> AddVehicleAsync(int teachingScheduleId, int vehicleId)
        {
            return await _teachingScheduleRepo.AddVehicleAsync(teachingScheduleId, vehicleId);
        }
        public async Task<TeachingScheduleModel> CreateAsync(TeachingScheduleModel teachingSchedule)
        {
            var teachingScheduleEntity = _mapper.Map<TeachingSchedule>(teachingSchedule);
            return await _teachingScheduleRepo.CreateAsync(teachingScheduleEntity);
        }
        public async Task<bool> CreateRangeBySlotAndWeekdayAsync(int slotId, string weekdays, int weekdayScheduleId
            , TeachingScheduleModel teachingSchedule)
        {
            return await _teachingScheduleRepo.CreateRangeBySlotAndWeekdayAsync(slotId, weekdays,
                weekdayScheduleId, teachingSchedule);
        }
        public async Task<bool> AddCoursePackageAsync(int teachingScheduleId, Guid coursePackageId)
        {
            return await _teachingScheduleRepo.AddCoursePackageAsync(teachingScheduleId, coursePackageId);
        }
        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAsync(Guid mentorId)
        {
            return await _teachingScheduleRepo.GetAllByMentorIdAsync(mentorId);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAndMemberIdAsync(Guid mentorId, Guid memberId)
        {
            return await _teachingScheduleRepo.GetAllByMentorIdAndMemberIdAsync(mentorId, memberId);
        }

        public async Task<TeachingScheduleModel> GetAsync(int teachingScheduleId)
        {
            return await _teachingScheduleRepo.GetAsync(teachingScheduleId);
        }

        public async Task<TeachingScheduleModel> GetByFilterAsync(TeachingScheduleFilter filters)
        {
            return await _teachingScheduleRepo.GetByFilterAsync(filters);
        }

        public async Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(int weekdayScheduleId, Guid mentorId, DateTime date, int slotId)
        {
            return await _teachingScheduleRepo.GetByMentorIdAndTeachingDateAsync(weekdayScheduleId, mentorId, date, slotId);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleAsync(int slotId,
            int weekDayScheduleId, Guid mentorId)
        {
            return await _teachingScheduleRepo.GetBySlotAndWeekDayScheduleAsync(slotId, weekDayScheduleId, mentorId);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleOfMemberAsync(int slotId, int weekDayScheduleId,
            Guid mentorId, Guid memberId)
        {
            return await _teachingScheduleRepo.GetBySlotAndWeekDayScheduleOfMemberAsync(slotId, weekDayScheduleId,
                mentorId, memberId);
        }
        public async Task<IEnumerable<TeachingScheduleModel>> GetAllAwaitScheduleByMentorAsync(int slotId, int weekDayScheduleId, Guid mentorId)
        {
            return await _teachingScheduleRepo.GetAllAwaitScheduleByMentorAsync(slotId, weekDayScheduleId, mentorId);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByTeachingDateAsync(DateTime date)
        {
            return await _teachingScheduleRepo.GetAllByTeachingDateAsync(date);
        }
        public async Task<IEnumerable<StaffModel>> GetAllAwaitScheduleMentor()
        {
            var teachingSchedules = await _teachingScheduleRepo.GetAllAwaitScheduleMentor();
            var mentors = teachingSchedules.Select(x => x.Staff).ToList();
            mentors = mentors.GroupBy(x => x.StaffId)
                             .Select(x => x.First())
                             .ToList();
            return mentors;
        }
        public async Task<TeachingScheduleModel> GetMemberScheduleByFilterAsync(LearningScheduleFilter filters, Guid memberId)
        {
            return await _teachingScheduleRepo.GetMemberScheduleByFilterAsync(filters, memberId);
        }
        public async Task<TeachingScheduleModel> ExistScheduleInOtherCoursesAsync(int slotId, DateTime teachingDate, Guid mentorId, Guid courseId)
        {
            return await _teachingScheduleRepo.ExistScheduleInOtherCoursesAsync(slotId, teachingDate, mentorId, courseId);
        }
        public async Task<bool> ApproveMentorAwaitSchedule(Guid mentorId)
        {
            return await _teachingScheduleRepo.ApproveMentorAwaitSchedule(mentorId);
        }

        public async Task<bool> AddRangeVehicleMentorSchedule(Guid mentorId, int vehicleId)
        {
            return await _teachingScheduleRepo.AddRangeVehicleMentorSchedule(mentorId,vehicleId);
        }

        public async Task<TeachingScheduleModel> GetFirstAwaitScheduleMentor(Guid mentorId)
        {
            return await _teachingScheduleRepo.GetFirstAwaitScheduleMentor(mentorId);
        }

        public async Task<bool> DenyMentorAwaitSchedule(Guid mentorId)
        {
            return await _teachingScheduleRepo.DenyMentorAwaitSchedule(mentorId);
        }
    }
}
