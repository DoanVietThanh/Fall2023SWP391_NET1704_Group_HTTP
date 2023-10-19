using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class CurriculumService : ICurriculumService
    {
        private readonly ICurriculumRepository _curriculumRepo;
        private readonly IMapper _mapper;

        public CurriculumService(ICurriculumRepository curriculumRepo,
            IMapper mapper)
        {
            _curriculumRepo = curriculumRepo;
            _mapper = mapper;
        }

        public async Task<bool> AddCourseAsync(int curriculumId, Guid courseId)
        {
            return await _curriculumRepo.AddCourseAsync(curriculumId, courseId);
        }

        public async Task<CurriculumModel> CreateAsync(CurriculumModel curriculum)
        {
            var curriculumEntity = _mapper.Map<Curriculum>(curriculum);
            return await _curriculumRepo.CreateAsync(curriculumEntity);
        }
    }
}
