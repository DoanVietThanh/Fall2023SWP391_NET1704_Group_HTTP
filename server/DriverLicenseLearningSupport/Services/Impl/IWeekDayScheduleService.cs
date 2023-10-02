using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IWeekDayScheduleService
    {
        Task<WeekdayScheduleModel> GetByDateAsync(DateTime date);
        Task<IEnumerable<WeekdayScheduleModel>> GetAllAsync();
        Task<bool> CreateRangeAsync(IEnumerable<WeekdayScheduleModel> weekdays);
    }
}
