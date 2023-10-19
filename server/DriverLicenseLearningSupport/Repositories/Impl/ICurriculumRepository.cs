using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ICurriculumRepository
    {
        Task<CurriculumModel> CreateAsync(Curriculum curriculum);
        Task<bool> AddCourseAsync(int curriculumId, Guid courseId);
    }
}
