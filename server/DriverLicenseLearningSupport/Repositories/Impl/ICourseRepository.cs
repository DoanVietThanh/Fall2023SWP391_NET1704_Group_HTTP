using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ICourseRepository
    {
        Task<CourseModel> CreateAsync(Course course);
        Task<CourseModel> GetAsync(Guid id );
        Task<CourseModel> GetHiddenCourseAsync(Guid id);
        Task<CourseModel> GetByMentorIdAsync(Guid mentorId);
        Task<IEnumerable<CourseModel>> GetAllAsync();
        Task<IEnumerable<CourseModel>> GetAllHiddenCourseAsync();
        Task<bool> AddCurriculumAsync(Guid courseId, int curriculumId);
        Task<bool> AddMentorAsync(Guid courseId, Guid mentorId);
        Task<bool> UpdateAsync(Guid id, Course course);
        Task<bool> UpdateCourseCurriculumAsync(Guid courseId, Curriculum curriculum);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> HideCourseAsync(Guid id);
        Task<bool> UnhideAsync(Guid id);
    }
}
