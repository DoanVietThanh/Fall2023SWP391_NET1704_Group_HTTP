using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IJobTitleService
    {
        Task<IEnumerable<JobTitleModel>> FindAllAsync();
        Task<JobTitleModel> FindByIdAsync(int id);
    }
}
