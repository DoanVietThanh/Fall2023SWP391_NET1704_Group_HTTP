using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IBlogRepository
    {
        Task<BlogModel> CreateAsync(Blog blog);

        Task<IEnumerable<BlogModel>> GetAllAsync();
    }
}
