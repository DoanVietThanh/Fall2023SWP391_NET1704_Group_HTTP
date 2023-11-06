using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IBlogService
    {
        Task<BlogModel> CreateAsync(BlogModel blog);

        Task<IEnumerable<BlogModel>> GetAllAsync();
        Task<IEnumerable<BlogModel>> GetBlogByIdAsync(int id);

        Task<IEnumerable<BlogModel>> GetBlogsByStaffId(Guid id);
        Task<bool> UpdateBlogAsync(BlogModel blog);
        Task<IEnumerable<BlogModel>> GetAllBlogWithoutCmt();
        Task<bool> DeleteBlogAsync(int blogId);
        Task<IEnumerable<BlogModel>> SearchBlogByAuthorAsync(string author);
        Task<IEnumerable<BlogModel>> SearchBlogByNameAsync(string name);
        Task<IEnumerable<BlogModel>> SearchBlogByTagId(int tagId);

    }
}
