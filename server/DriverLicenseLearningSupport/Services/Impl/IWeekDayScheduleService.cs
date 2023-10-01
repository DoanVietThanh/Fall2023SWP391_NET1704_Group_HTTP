using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IWeekDayScheduleService
    {
        Task<bool> CreateRangeAsync(IEnumerable<WeekdayScheduleModel> weekdays);
    }
}
