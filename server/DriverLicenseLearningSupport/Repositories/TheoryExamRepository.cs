using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class TheoryExamRepository : ITheoryExamRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public TheoryExamRepository(DriverLicenseLearningSupportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddQuesitonAsync(int theoryExamId, int questionId)
        {
            var theoryEntity = await _context.TheoryExams.Where(x => x.TheoryExamId == theoryExamId)
                .FirstOrDefaultAsync();
            if (theoryEntity is not null)
            {
                var quesitonEntity = await _context.Questions.Where(q => q.QuestionId == questionId)
                    .FirstOrDefaultAsync();
                theoryEntity.Questions.Add(quesitonEntity);
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }

        public async Task<TheoryExamModel> CreateAsync(TheoryExam theoryExam)
        {
            await _context.TheoryExams.AddAsync(theoryExam);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSuccess)
            {
                var theoryEntity = await _context.TheoryExams.OrderByDescending(x => x.TheoryExamId)
                    .FirstOrDefaultAsync();
                theoryExam.TheoryExamId = Convert.ToInt32(theoryEntity.TheoryExamId);
                
            }
                return _mapper.Map<TheoryExamModel>(theoryExam);
            
        }

    }
}
