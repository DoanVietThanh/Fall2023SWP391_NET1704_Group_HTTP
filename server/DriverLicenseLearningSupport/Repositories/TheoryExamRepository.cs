using AutoMapper;
using DocumentFormat.OpenXml.InkML;
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

                // set null
                theoryExam.LicenseType.Questions = new List<Question>();

                List<Question> questions = new List<Question>();

                foreach (var question in list)
                {
                    var q = await _context.Questions.Where(x => x.QuestionId == question.QuestionId)
                        .FirstOrDefaultAsync();
                    theoryEntity.Questions.Add(q);
                }
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<TheoryExamModel>(theoryExam);

        }

        public async Task<IEnumerable<TheoryExamModel>> GetAllAsync()
        {
            var theoryExams = await _context.TheoryExams.Select(x => new TheoryExam()
            {
                TheoryExamId = x.TheoryExamId,
                LicenseTypeId = x.LicenseTypeId,
                TotalAnswerRequired = x.TotalAnswerRequired,
                TotalTime = x.TotalTime,
                TotalQuestion = x.TotalQuestion,
                Questions = x.Questions.Select(q => new Question()
                {
                    QuestionId = q.QuestionId,
                    Image = q.Image,
                    LicenseType = q.LicenseType,
                    IsParalysis = q.IsParalysis,
                    QuestionAnswers = q.QuestionAnswers.Select(a => new QuestionAnswer()
                    {

                        Answer = a.Answer,
                        QuestionAnswerId = a.QuestionAnswerId,
                        IsTrue = a.IsTrue
                    }).ToList()
                }).ToList()
            }).ToListAsync();

            return _mapper.Map<IEnumerable<TheoryExamModel>>(theoryExams);
        }

        public async Task<bool> IsExamQuestion(int questionId)
        {
            //lay toan bo cau hoi cua moi de
            /*    var theoryEntites = await _context.TheoryExams.Select(x => new TheoryExam()
                {
                    TheoryExamId = x.TheoryExamId,
                    LicenseTypeId = x.LicenseTypeId,
                    TotalAnswerRequired = x.TotalAnswerRequired,
                    TotalTime = x.TotalTime,
                    TotalQuestion = x.TotalQuestion,
                    Questions = x.Questions.Select(q => new Question()
                    {
                        QuestionId = q.QuestionId
                    }).ToList()
                }).ToListAsync();
            */
            var theoryEntities = await _context.TheoryExams.Include(x => x.Questions).ToListAsync();

            foreach(var tEntity in theoryEntities)
            {
                var quest = tEntity.Questions.Where(x => x.QuestionId == questionId).FirstOrDefault();
                if (quest is not null) return true;
            }

            /*var question = theoryEntites.Select(x => x.Questions.Where(x => x.QuestionId == questionId).FirstOrDefault())
                                        .FirstOrDefault();
            if (theoryEntity is not null)
            {
                return true;
            }*/
            return false;
        }
        public async Task<bool> HasHistory(int id)
        {
            var examHistories = await _context.ExamHistories.Where(x => x.TheoryExamId == id).ToListAsync();
            if (examHistories is not null || examHistories.Count() != 0)
            {
                return false;
            }
            return true;
        }

        public async Task<TheoryExamModel> GetByIdAsync(int id)
        {
            var theoryEntity = await _context.TheoryExams.Where(x => x.TheoryExamId == id).
                Select(x => new TheoryExam()
                {
                    TheoryExamId = x.TheoryExamId,
                    LicenseTypeId = x.LicenseTypeId,
                    TotalAnswerRequired = x.TotalAnswerRequired,
                    TotalTime = x.TotalTime,
                    TotalQuestion = x.TotalQuestion,
                    Questions = x.Questions.Select(q => new Question()
                    {
                        QuestionId = q.QuestionId,
                        Image = q.Image,
                        QuestionAnswerDesc = q.QuestionAnswerDesc,
                        LicenseType = q.LicenseType,
                        IsParalysis = q.IsParalysis,
                        QuestionAnswers = q.QuestionAnswers.Select(a => new QuestionAnswer()
                        {
                            Answer = a.Answer,
                            QuestionAnswerId = a.QuestionAnswerId,
                            IsTrue = a.IsTrue
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();

            return _mapper.Map<TheoryExamModel>(theoryEntity);
        }

        public async Task<IEnumerable<TheoryExamModel>> GetByLicenseTypeIdAsync(int licenseTypeId)
        {
            var theoryExams = await _context.TheoryExams.Where(te => te.LicenseTypeId == licenseTypeId)
                .ToListAsync();
            if (theoryExams is null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<TheoryExamModel>>(theoryExams);

        }

        
        public async Task<bool> RemoveTheoryExam(int id)
        {
            // Retrieve the TheoryExam object by its ID
            var theoryExam = await _context.TheoryExams.Include(te => te.Questions).FirstOrDefaultAsync(e => e.TheoryExamId == id);

            // Check if the TheoryExam exists
            if (theoryExam != null)
            {
                // Clear the relationship between TheoryExam and Questions
                theoryExam.Questions.Clear();
                await _context.SaveChangesAsync(); // Save changes to the database
            }

            if (theoryExam is null) { return false; }

            _context.TheoryExams.Remove(theoryExam);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
