using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ITagService
    {
        Task<TagModel> CreateAsync(TagModel tag);
        Task<IEnumerable<TagModel>> GetAllAsync();
        Task<TagModel> GetTagById(int id);
        Task<bool> ExistTag(string name);
    }
}
