using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICourseService
    {
        Task<CourseModel> CreateAsync(CourseModel course);
        Task<CourseModel> GetAsync(Guid id);
        Task<CourseModel> GetHiddenCourseAsync(Guid id);
        Task<CourseModel> GetByMentorIdAsync(Guid mentorId);
        Task<CourseModel> GetByMentorIdAndCourseIdAsync(Guid mentorId, Guid courseId);
        Task<CoursePackageModel> CreatePackageAsync(CoursePackageModel coursePackage);
        Task<CoursePackageModel> GetPackageAsync(Guid packageId);
        Task<IEnumerable<CourseModel>> GetAllAsync();
        Task<IEnumerable<CourseModel>> GetAllMentorCourseAsync(Guid mentorId);
        Task<IEnumerable<CourseModel>> GetAllHiddenCourseAsync();
        Task<bool> AddCurriculumAsync(Guid courseId, CurriculumModel curriculum);
        Task<bool> AddMentorAsync(Guid courseId, Guid mentorId);
        Task<bool> UpdateAsync(Guid id, CourseModel course);
        Task<bool> UpdateCourseCurriculumAsync(Guid courseId, CurriculumModel curriculum);
        Task<bool> UpdatePackageAsync(Guid packageId, CoursePackageModel package);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeletePackageAsync(Guid id);
        Task<bool> HideCourseAsync(Guid id);
        Task<bool> UnhideAsync(Guid id);
    }
}
