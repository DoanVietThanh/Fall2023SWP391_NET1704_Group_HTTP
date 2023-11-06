using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IBlogRepository
    {
        Task<BlogModel> CreateAsync(Blog blog);

        Task<IEnumerable<BlogModel>> GetAllAsync();

        Task<IEnumerable<BlogModel>> GetBlogByIdAsync(int id);

        Task<IEnumerable<BlogModel>> GetBlogsByStaffId(Guid id);

        Task<bool> UpdateBlogAsync(BlogModel blog);

        Task<IEnumerable<BlogModel>> GetAllBlogWithoutCmt();

        Task<bool> DeleteBlogAsync(int blogId);

        Task<IEnumerable<BlogModel>> SearchBlogByNameAsync(string name);

        Task<IEnumerable<BlogModel>> SearchBlogByAuthorAsync(string author);

        Task<IEnumerable<BlogModel>> SearchBlogByTagId(int tagId);

    }
}
