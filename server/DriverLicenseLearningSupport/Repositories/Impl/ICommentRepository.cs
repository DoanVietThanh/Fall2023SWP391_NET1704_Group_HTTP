using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ICommentRepository
    {
        Task<CommentModel> CreateAsync(Comment comment);

        Task<bool> DeleteAsync(int id);

        Task<bool> UpdateAsync(CommentModel comment);
    }
}
