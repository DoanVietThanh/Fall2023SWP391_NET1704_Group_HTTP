using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;

namespace DriverLicenseLearningSupport.Repositories
{
    public class ExamGradeRepository : IExamGradeRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public ExamGradeRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ExamGradeModel> CreateAsync(ExamGrade entity)
        {
            await _context.ExamGrades.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ExamGradeModel>(entity);
        }
    }
}
