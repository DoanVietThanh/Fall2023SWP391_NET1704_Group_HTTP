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
        public async Task<CourseModel> GetHiddenCourseAsync(Guid id) 
        {
            return await _courseRepo.GetHiddenCourseAsync(id);
        }
        public async Task<CourseModel> GetByMentorIdAsync(Guid mentorId)
        {
            return await _courseRepo.GetByMentorIdAsync(mentorId);
        }
        public async Task<CourseModel> GetByMentorIdAndCourseIdAsync(Guid mentorId, Guid courseId)
        {
            return await _courseRepo.GetByMentorIdAndCourseIdAsync(mentorId, courseId);
        }
        public async Task<IEnumerable<CourseModel>> GetAllAsync()
        {
            return await _courseRepo.GetAllAsync();
        }
        public async Task<IEnumerable<CourseModel>> GetAllMentorCourseAsync(Guid mentorId)
        {
            return await _courseRepo.GetAllMentorCourseAsync(mentorId);
        }
        public async Task<IEnumerable<CourseModel>> GetAllHiddenCourseAsync()
        {
            return await _courseRepo.GetAllHiddenCourseAsync();
        }
        public async Task<bool> UpdateAsync(Guid id, CourseModel course)
        {
            var courseEntity = _mapper.Map<Course>(course);
            return await _courseRepo.UpdateAsync(id, courseEntity);
        }
        public async Task<bool> UpdateCourseCurriculumAsync(Guid courseId, CurriculumModel curriculum)
        {
            var curriculumEntity = _mapper.Map<Curriculum>(curriculum);
            return await _courseRepo.UpdateCourseCurriculumAsync(courseId, curriculumEntity);
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
        public async Task<bool> AddMentorAsync(Guid courseId, Guid mentorId)
        {
            return await _courseRepo.AddMentorAsync(courseId, mentorId);
        }
        public async Task<bool> UnhideAsync(Guid id)
        {
            return await _courseRepo.UnhideAsync(id);
        }

    }
}
