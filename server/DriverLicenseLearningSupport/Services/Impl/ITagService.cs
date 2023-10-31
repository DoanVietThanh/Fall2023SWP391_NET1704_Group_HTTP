using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ITagService
    {
        Task<TagModel> CreateAsync(TagModel tag);
    }
}
