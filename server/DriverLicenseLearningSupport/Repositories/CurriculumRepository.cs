using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class CurriculumRepository : ICurriculumRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public CurriculumRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddCourseAsync(int curriculumId, Guid courseId)
        {
            var curriculumEntity = await _context.Curricula.Where(x => x.CurriculumId == curriculumId)
                                                     .FirstOrDefaultAsync();
            if (curriculumEntity is not null) 
            {
                var courseEntity = await _context.Courses.Where(x => x.CourseId == courseId.ToString())
                                                         .FirstOrDefaultAsync();
                curriculumEntity.Courses.Add(courseEntity);

                return await _context.SaveChangesAsync() > 0 ? true : false;
            }

            return false;
        }

        public async Task<CurriculumModel> CreateAsync(Curriculum curriculum)
        {
            await _context.Curricula.AddAsync(curriculum);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSucess)
            {
                Curriculum? curriculumEntity = await _context.Curricula.OrderByDescending(x => x.CurriculumId)
                                                                     .FirstOrDefaultAsync();
                curriculum = curriculumEntity;

                return _mapper.Map<CurriculumModel>(curriculum);
            }
            return null;
        }
    }
}
