using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly ICurriculumService _curriculumService;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepo,
            ICurriculumService curriculumService,
            IMapper mapper)
        {
            _courseRepo = courseRepo;
            _curriculumService = curriculumService;
            _mapper = mapper;
        }

        public async Task<CourseModel> CreateAsync(CourseModel course)
        {
            var courseEntity = _mapper.Map<Course>(course);
            return await _courseRepo.CreateAsync(courseEntity);
        }
        public async Task<CourseModel> GetAsync(Guid id)
        {
            return await _courseRepo.GetAsync(id);
        }
        public async Task<IEnumerable<CourseModel>> GetAllAsync()
        {
            return await _courseRepo.GetAllAsync();
        }
        public async Task<IEnumerable<CourseModel>> GetAllHiddenCourseAsync()
        {
            return await _courseRepo.GetAllHiddenCourseAsync();
        }
        public async Task<bool> UpdateAsync(Guid id, CourseModel course)
        {
            return await _courseRepo.UpdateAsync(id, course);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _courseRepo.DeleteAsync(id);
        }
        public async Task<bool> HideCourseAsync(Guid id)
        {
            return await _courseRepo.HideCourseAsync(id);
        }
        public async Task<bool> AddCurriculumAsync(Guid courseId, CurriculumModel curriculum)
        {
            // create new curriculum
            var createdCurriculum = await _curriculumService.CreateAsync(curriculum);
            // add curriculum to courses
            bool addCurriculumSucess = await _courseRepo.AddCurriculumAsync(courseId, createdCurriculum.CurriculumId);
            // success
            if (addCurriculumSucess) return true;
            
            // cause error
            return false;
        }
    }
}
