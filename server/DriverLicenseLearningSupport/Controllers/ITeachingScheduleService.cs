using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Controllers
{
    public interface ITeachingScheduleService
    {
        Task<TeachingScheduleModel> CreateAsync(TeachingScheduleModel teachingSchedule);
        Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorId(Guid mentorId);
        Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleAsync(int slotId, int weekDayScheduleId, Guid mentorId);
        Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(Guid mentorId, DateTime date);
    }
}