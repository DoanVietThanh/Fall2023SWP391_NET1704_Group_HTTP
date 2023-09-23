using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace DriverLicenseLearningSupport.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public AnswerRepository(DriverLicenseLearningSupportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAnswerAsync(QuestionAnswer answer)
        {
            _context.QuestionAnswers.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AnswerModel>> getAllAnwser()
        {
            var answers = await _context.QuestionAnswers.ToListAsync();
            return _mapper.Map<IEnumerable<AnswerModel>>(answers);
        }

        public async Task<AnswerModel> GetAnswerAsync(int answerId)
        {
            var answer = await _context.QuestionAnswers.FindAsync(answerId);
            return _mapper.Map<AnswerModel>(answer);
        }

        public async Task<IEnumerable<AnswerModel>> getAnswserofQuestion(int questionId)
        {
            var listAnswer = await _context.QuestionAnswers.Where(x => x.QuestionAnswerId == questionId).ToListAsync();
            return _mapper.Map<IEnumerable<AnswerModel>>(listAnswer);
        }

        public async Task RemoveAnswerAsync(int answerId)
        {
            var removeAnswer = _context.QuestionAnswers.SingleOrDefault(a => a.QuestionAnswerId == answerId);
            if(removeAnswer != null) 
            {
                _context.QuestionAnswers.Remove(removeAnswer);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
