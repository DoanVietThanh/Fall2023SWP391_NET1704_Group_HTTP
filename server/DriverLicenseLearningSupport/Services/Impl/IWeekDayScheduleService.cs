using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IWeekDayScheduleService
    {
        Task<WeekdayScheduleModel> GetAsync(int id);
        Task<WeekdayScheduleModel> GetByDateAsync(DateTime date);
        Task<WeekdayScheduleModel> GetByDateAndCourseId(DateTime date, Guid courseId);
        Task<IEnumerable<WeekdayScheduleModel>> GetAllAsync();
        Task<IEnumerable<WeekdayScheduleModel>> GetAllByCourseId(Guid courseId);
        Task<bool> CreateRangeAsync(IEnumerable<WeekdayScheduleModel> weekdays);
    }
}
