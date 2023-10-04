using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ITeachingScheduleRepository
    {
        Task<TeachingScheduleModel> CreateAsync(TeachingSchedule teachingSchedule);
        Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorId(Guid mentorId);
        Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleAsync(int slotId, int weekDayScheduleId, Guid mentorId);
        Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(Guid mentorId, DateTime date);
    }
}
