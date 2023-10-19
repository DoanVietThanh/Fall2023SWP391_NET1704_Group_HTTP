using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICurriculumService
    {
        Task<CurriculumModel> CreateAsync(CurriculumModel curriculum);
        Task<bool> AddCourseAsync(int curriculumId, Guid courseId);
    }
}
