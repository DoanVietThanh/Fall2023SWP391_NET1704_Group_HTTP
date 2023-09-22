using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IJobTitleRepository
    {
        Task<IEnumerable<JobTitleModel>> GetAllAsync();
        Task<JobTitleModel> GetAsync(int id);
    }
 }
