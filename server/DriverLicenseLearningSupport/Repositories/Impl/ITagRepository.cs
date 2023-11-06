using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ITagRepository
    {
        Task<TagModel> CreateAsync(Tag tag);

        Task<IEnumerable<TagModel>> GetAllAsync();

        Task<TagModel> GetTagById(int id);

        Task<bool> ExistTag(string name);
    }
}
