using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICommentService
    {
        Task<CommentModel> CreateAsync(CommentModel comment);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(CommentModel comment);
    }
}
