using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace DriverLicenseLearningSupport.Repositories
{
    public class PracticeExamRepository : IPracticeExamRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public PracticeExamRepository(DriverLicenseLearningSupportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreatePracticeExamAsync(PracticeExam practiceExam)
        {
            var practicalExamEntity = _mapper.Map<PracticeExam>(practiceExam);
            _context.PracticeExams.Add(practicalExamEntity);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task DeletePraticeExamAsynx(int practiceExamId)
        {
            var removeExam = _context.PracticeExams.SingleOrDefault(x => x.PracticeExamId == practiceExamId);
            if (removeExam != null) 
            {
                _context.PracticeExams.Remove(removeExam);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<PracticeExam>> GetAllPracticeExamAsync()
        {
            var practiceExams = await _context.PracticeExams.ToListAsync();
            return practiceExams;
        }

        public async Task<PracticeExamModel> GetPracticeExamAsync(int practiceExamId)
        {
            var practiceExam = await _context.PracticeExams.FindAsync(practiceExamId);
            return _mapper.Map<PracticeExamModel>(practiceExam);
        }

        public async Task UpdatePracticeExamAsync(int practiceExamId, PracticeExamModel practiceExamModel)
        {
            if (practiceExamId == practiceExamModel.PracticeExamId) 
            {
                var practiceExams = _mapper.Map<PracticeExam>(practiceExamModel);
                _context.PracticeExams.Update(practiceExams);
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
