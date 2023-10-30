using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ITagRepository
    {
        Task<TagModel> CreateAsync(Tag tag);
    }
}
