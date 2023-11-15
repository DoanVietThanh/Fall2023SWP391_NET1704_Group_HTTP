using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class ExamHistoryRepostory : IExamHistoryRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public ExamHistoryRepostory(DriverLicenseLearningSupportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExamHistoryModel> CreateAsync(ExamHistory examHistory)
        {
            await _context.AddAsync(examHistory);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSuccess)
            {
                var examHistoryEntity = await _context.ExamHistories.OrderByDescending(x => x.ExamHistoryId)
                    .FirstOrDefaultAsync();
                examHistory.ExamHistoryId = examHistoryEntity.ExamHistoryId;
            }
            return _mapper.Map<ExamHistoryModel>(examHistory);

        }

        public async Task<ExamHistoryModel> GetHistoryDetailAsync(string MemberId, int TheoryExamId, DateTime joinDate)
        {
            var ExamHistory = await _context.ExamHistories.Where(eh => eh.MemberId.Equals(MemberId)
            && eh.TheoryExamId == TheoryExamId && eh.Date.Equals(joinDate)).FirstOrDefaultAsync();
            return _mapper.Map<ExamHistoryModel>(ExamHistory);
        }

        public async Task<IEnumerable<ExamHistoryModel>> GetAllByMemberIdAsysn(string memberId)
        {
            var MyHistories = await _context.ExamHistories.Where(x => x.MemberId.Equals(memberId))
                .Select(x => new ExamHistory { 
                    ExamHistoryId = x.ExamHistoryId,
                    MemberId = x.MemberId,
                    TotalGrade = x.TotalGrade,
                    TotalRightAnswer = x.TotalRightAnswer,
                    TotalQuestion = x.TotalQuestion,
                    TotalTime = x.TotalTime,
                    WrongParalysisQuestion = x.WrongParalysisQuestion,
                    IsPassed = x.IsPassed,
                    Date = x.Date,
                    TheoryExam = new TheoryExam { 
                        TheoryExamId = x.TheoryExam.TheoryExamId,
                        LicenseType = x.TheoryExam.LicenseType
                    }
                })
                .ToListAsync();


            return _mapper.Map<IEnumerable<ExamHistoryModel>>(MyHistories);
        }

        public async Task<IEnumerable<ExamHistoryModel>> GetAllExamHistory()
        {
            var examHistories = await _context.ExamHistories.Include(x => x.TheoryExam).ToListAsync();
            return _mapper.Map<IEnumerable<ExamHistoryModel>>(examHistories);
        }
    }
}
