using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IJobTitleRepository
    {
        Task<IEnumerable<JobTitleModel>> FindAllAsync();
        Task<JobTitleModel> FindByIdAsync(int id);
    }
 }
