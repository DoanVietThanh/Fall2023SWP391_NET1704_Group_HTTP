    using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace DriverLicenseLearningSupport.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public QuestionRepository(DriverLicenseLearningSupportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> CreateQuesAsync(QuestionModel question)
        {
            var questionBank = _mapper.Map<QuestionBank>(question);
            _context.QuestionBanks.Add(questionBank);
            var result = await _context.SaveChangesAsync();
            return questionBank.QuestionId;
        }

        public async Task<IEnumerable<QuestionModel>> GetAllQuesAsync()
        {
            var questions = await _context.QuestionBanks.ToListAsync();
            return _mapper.Map<IEnumerable<QuestionModel>>(questions);
        }

        public async Task<QuestionModel> GetQuesAsync(int questionId)
        {
            var question = await _context.QuestionBanks.FindAsync(questionId);
            return _mapper.Map<QuestionModel>(question);
        }

        public async Task RemoveQuesAsync(int questionId)
        {
            var deleteQuestion = _context.QuestionBanks.SingleOrDefault(q => q.QuestionId == questionId);
            if (deleteQuestion != null) 
            {
                _context.QuestionBanks.Remove(deleteQuestion);
                await _context.SaveChangesAsync();
            }
        }

       
    }
}
