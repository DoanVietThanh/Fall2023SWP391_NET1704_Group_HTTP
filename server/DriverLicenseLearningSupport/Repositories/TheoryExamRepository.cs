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
            var list = theoryExam.Questions;
            theoryExam.Questions = new List<Question>();
            await _context.TheoryExams.AddAsync(theoryExam);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSuccess)
            {
                var theoryEntity = await _context.TheoryExams.OrderByDescending(x => x.TheoryExamId)
                    .FirstOrDefaultAsync();
                //theoryEntity.Questions.Add(list);
                theoryExam.TheoryExamId = Convert.ToInt32(theoryEntity.TheoryExamId);

                List<Question> questions = new List<Question>();

                foreach(var question in list)
                {
                    var q = await _context.Questions.Where(x => x.QuestionId == question.QuestionId)
                        .FirstOrDefaultAsync();
                    theoryEntity.Questions.Add(q);
                }
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<TheoryExamModel>(theoryExam);
            
        }

        public async Task<bool> IsExamQuestion(int questionId)
        {
            //lay toan bo cau hoi cua moi de
            var theoryEntites = await _context.TheoryExams.Select(x => new TheoryExam() { 
                                                                TheoryExamId = x.TheoryExamId,
                                                                LicenseTypeId = x.LicenseTypeId,
                                                                TotalAnswerRequired = x.TotalAnswerRequired,
                                                                TotalTime = x.TotalTime,
                                                                TotalQuestion = x.TotalQuestion,
                                                                Questions = x.Questions.Select(q => new Question() { 
                                                                    QuestionId = q.QuestionId
                                                                }).ToList()
                                                            }).ToListAsync();

            var question = theoryEntites.Select(x => x.Questions.Where(x => x.QuestionId == questionId).FirstOrDefault())
                                        .FirstOrDefault();
            if (question is not null) 
            {
                return true;
            }
            return false;
        }
    }
}
