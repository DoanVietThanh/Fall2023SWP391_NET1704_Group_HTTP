using AutoMapper;
using DriverLicenseLearningSupport.Controllers;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;

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

        public async Task<TeachingScheduleModel> CreateAsync(TeachingScheduleModel teachingSchedule)
        {
            var teachingScheduleEntity = _mapper.Map<TeachingSchedule>(teachingSchedule);
            return await _teachingScheduleRepo.CreateAsync(teachingScheduleEntity);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorId(Guid mentorId)
        {
            return await _teachingScheduleRepo.GetAllByMentorId(mentorId);
        }

        public async Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(Guid mentorId, DateTime date)
        {
            return await _teachingScheduleRepo.GetByMentorIdAndTeachingDateAsync(mentorId, date);
        }

        public async Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleAsync(int slotId,
            int weekDayScheduleId, Guid mentorId)
        {
            return await _teachingScheduleRepo.GetBySlotAndWeekDayScheduleAsync(slotId, weekDayScheduleId, mentorId);
        }
    }
}
