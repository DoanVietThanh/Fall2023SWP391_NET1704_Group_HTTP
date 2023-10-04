using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IWeekDayScheduleRepository
    {
        Task<WeekdayScheduleModel> GetByDateAsync(DateTime date);
        Task<IEnumerable<WeekdayScheduleModel>> GetAllAsync();
        Task<bool> CreateRangeAsync(IEnumerable<WeekdaySchedule> weekdays);
    }
}
