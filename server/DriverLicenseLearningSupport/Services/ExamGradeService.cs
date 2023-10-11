using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;
using System.Runtime.InteropServices;

namespace DriverLicenseLearningSupport.Services
{
    public class ExamGradeService : IExamGradeService
    {
        private readonly IMapper _mapper;
        private readonly IExamGradeRepository _examGradeRepository;

        public ExamGradeService(IExamGradeRepository examGradeRepository, IMapper mapper) 
        {
            _mapper = mapper;
            _examGradeRepository = examGradeRepository;
        }
        public async Task<ExamGradeModel> CreateAsync(ExamGradeModel model)
        {
            var examGradeEntity = _mapper.Map<ExamGrade>(model);
            return await _examGradeRepository.CreateAsync(examGradeEntity);
        }

        public async Task<List<ExamGradeModel>> GetAllByTheoryExamIdandEmailAsync(string Email, int TheoryExamId, DateTime StartedDate)
        {
            return await _examGradeRepository.GetAllByTheoryExamIdandEmailAsync(Email, TheoryExamId,StartedDate) ;
        }
    }
}
