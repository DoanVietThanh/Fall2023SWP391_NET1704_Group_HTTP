using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

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

        public async Task<TeachingScheduleModel> GetMemberScheduleByFilterAsync(LearningScheduleFilter filters, Guid memberId)
        {
            return await _teachingScheduleRepo.GetMemberScheduleByFilterAsync(filters, memberId);
        }
    }
}
