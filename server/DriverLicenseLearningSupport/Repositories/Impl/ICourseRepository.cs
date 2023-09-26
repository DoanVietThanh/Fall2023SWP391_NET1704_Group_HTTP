using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ICourseRepository
    {
        Task<CourseModel> CreateAsync(Course course);
        Task<CourseModel> GetAsync(Guid id );
        Task<IEnumerable<CourseModel>> GetAllAsync();
        Task<IEnumerable<CourseModel>> GetAllHiddenCourseAsync();
        Task<bool> AddCurriculumAsync(Guid courseId, int curriculumId);
        Task<bool> UpdateAsync(Guid id, CourseModel course);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> HideCourseAsync(Guid id);
    }
}
