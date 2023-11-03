using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ITeachingScheduleService
    {
        Task<TeachingScheduleModel> CreateAsync(TeachingScheduleModel teachingSchedule);
        Task<bool> CreateRangeBySlotAndWeekdayAsync(int slotId, string weekdays, int weekdayScheduleId,
            TeachingScheduleModel teachingSchedule);
        Task<bool> AddCoursePackageAsync(int teachingScheduleId, Guid coursePackageId);
        Task<TeachingScheduleModel> GetAsync(int teachingScheduleId);
        Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAndMemberIdAsync(Guid mentorId, Guid memberId);
        Task<IEnumerable<TeachingScheduleModel>> GetAllByMentorIdAsync(Guid mentorId);
        Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleAsync(int slotId, int weekDayScheduleId, Guid mentorId);
        Task<IEnumerable<TeachingScheduleModel>> GetBySlotAndWeekDayScheduleOfMemberAsync(int slotId, int weekDayScheduleId, Guid mentorId, Guid memberId);
        Task<IEnumerable<TeachingScheduleModel>> GetAllByTeachingDateAsync(DateTime date);
        Task<IEnumerable<TeachingScheduleModel>> GetAllAwaitScheduleByMentorAsync(int slotId, int weekDayScheduleId, Guid mentorId);
        Task<IEnumerable<TeachingScheduleModel>> GetAllWeekdayScheduleAsync(int weekDayScheduleId);
        Task<IEnumerable<StaffModel>> GetAllAwaitScheduleMentor();
        Task<TeachingScheduleModel> GetByMentorIdAndTeachingDateAsync(int weekdayScheduleId, Guid mentorId, DateTime date, int slotId);
        Task<TeachingScheduleModel> GetByFilterAsync(TeachingScheduleFilter filters);
        Task<TeachingScheduleModel> GetMemberScheduleByFilterAsync(LearningScheduleFilter filters, Guid memberId);
        Task<TeachingScheduleModel> ExistScheduleInOtherCoursesAsync(int slotId, DateTime teachingDate, Guid mentorId, Guid courseId);
        Task<TeachingScheduleModel> GetFirstAwaitScheduleMentor(Guid mentorId);
        Task<bool> AddRollCallBookAsync(int teachingScheduleId, RollCallBookModel rcbModel);
        Task<bool> AddVehicleAsync(int teachingScheduleId, int vehicleId);
        Task<bool> ApproveMentorAwaitSchedule(Guid mentorId);
        Task<bool> AddRangeVehicleMentorSchedule(Guid mentorId, int vehicleId);
        Task<bool> DenyMentorAwaitSchedule(Guid mentorId);
    }
}