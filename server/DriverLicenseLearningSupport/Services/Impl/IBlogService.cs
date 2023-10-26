using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IBlogService
    {
        Task<BlogModel> CreateAsync(BlogModel blog);

        Task<IEnumerable<BlogModel>> GetAllAsync();
    }
}
