using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IWeekDayScheduleRepository
    {
        Task<bool> CreateRangeAsync(IEnumerable<WeekdaySchedule> weekdays);
    }
}
