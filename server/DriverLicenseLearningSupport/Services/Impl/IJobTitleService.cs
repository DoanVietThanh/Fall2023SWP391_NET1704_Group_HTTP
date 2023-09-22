using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IJobTitleService
    {
        Task<IEnumerable<JobTitleModel>> GetAllAsync();
        Task<JobTitleModel> GetAsync(int id);
    }
}
