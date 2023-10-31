using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IBlogRepository
    {
        Task<BlogModel> CreateAsync(Blog blog);

        Task<IEnumerable<BlogModel>> GetAllAsync();

        Task<IEnumerable<BlogModel>> GetBlogByIdAsync(int id);

        Task<IEnumerable<BlogModel>> GetBlogsByStaffId(string id);
    }
}
