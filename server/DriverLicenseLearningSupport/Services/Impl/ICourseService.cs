﻿using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICourseService
    {
        Task<CourseModel> CreateAsync(CourseModel course);
        Task<CourseModel> GetAsync(Guid id);
        Task<IEnumerable<CourseModel>> GetAllAsync();
        Task<IEnumerable<CourseModel>> GetAllHiddenCourseAsync();
        Task<bool> AddCurriculumAsync(Guid courseId, CurriculumModel curriculum);
        Task<bool> UpdateAsync(Guid id, CourseModel course);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> HideCourseAsync(Guid id);
        
    }
}