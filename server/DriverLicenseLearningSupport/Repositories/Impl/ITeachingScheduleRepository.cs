using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ITeachingScheduleRepository
    {
        Task<TeachingScheduleModel> CreateAsync(TeachingSchedule teachingSchedule);
        Task<bool> CreateRangeBySlotAndWeekdayAsync(int slotId, string weekdays, int weekdayScheduleId, TeachingScheduleModel teachingSchedule);
        Task<TeachingScheduleModel> GetAsync(int teachingScheduleId);
        Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAndMemberIdAsync(Guid mentorId, Guid memberId);
        Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAsync(Guid mentorId);
        Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleAsync(int slotId, int weekDayScheduleId, Guid mentorId);
        Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleOfMemberAsync(int slotId, int weekDayScheduleId, Guid mentorId, Guid memberId);
        Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(int weekdayScheduleId, Guid mentorId, DateTime date, int slotId);
        Task<TeachingScheduleModel> GetByFilterAsync(TeachingScheduleFilter filters);
        Task<TeachingScheduleModel> GetMemberScheduleByFilterAsync(LearningScheduleFilter filters, Guid memberId);
        Task<TeachingScheduleModel> ExistScheduleInOtherCoursesAsync(int slotId, DateTime teachingDate, Guid mentorId, Guid courseId);
        Task<bool> AddRollCallBookAsync(int teachingScheduleId, RollCallBook rcbModel);
        Task<bool> AddVehicleAsync(int teachingScheduleId, int vehicleId);
    }
}
