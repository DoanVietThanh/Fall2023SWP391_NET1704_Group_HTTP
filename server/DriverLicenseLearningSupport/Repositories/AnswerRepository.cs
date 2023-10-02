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

        public async Task<AnswerModel> CreateAsync(QuestionAnswer answer)
        {
            await _context.QuestionAnswers.AddAsync(answer);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSuccess)
            {
                var answerEntity = await _context.QuestionAnswers.OrderByDescending(x => x.QuestionAnswerId)
                    .FirstOrDefaultAsync();
                answer.QuestionAnswerId = Convert.ToInt32(answerEntity.QuestionAnswerId);
            }
            return _mapper.Map<AnswerModel>(answer);

        }

        public async Task<IEnumerable<AnswerModel>> CreateRangeAsync(IEnumerable<QuestionAnswer> answers)
        {
            await _context.QuestionAnswers.AddRangeAsync(answers);

            var isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!isSuccess)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<AnswerModel>>(answers);
        }

        public async Task<bool> DeleteAnswerAsync(int answerId)
        {
            // find the answer to delete
            var answer = await _context.QuestionAnswers.Where(x => x.QuestionAnswerId == answerId)
                .FirstOrDefaultAsync();
            // can not find the answer case
            if (answer is null) return false;

            // remove answer
            _context.QuestionAnswers.Remove(answer);

            return await _context.SaveChangesAsync() > 0 ? true : false;

        }

        public async Task<bool> DeleteAnswersByQuestionIdAsync(int quesitonId)
        {
            // find all the answer to delete
            var answers = await _context.QuestionAnswers.Where(x => x.QuestionId == quesitonId)
                .ToListAsync();
            // if their is no answers
            if(answers is null) return false;
            // remove all answers of the questions
            _context.QuestionAnswers.RemoveRange(answers);
            //save in the db
            return await _context.SaveChangesAsync() > 0 ? true : false ;
        }

        public async Task<IEnumerable<AnswerModel>> GetAllByQuestionId(int questionId)
        {
            var answers = await _context.QuestionAnswers.Where(x => x.QuestionId == questionId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<AnswerModel>>(answers);
        }

        public async Task<AnswerModel> GetByAnswerIdAsync(int answerId)
        {
            var answerEntity = await _context.QuestionAnswers.Where(a => a.QuestionAnswerId == answerId)
                .FirstOrDefaultAsync();
            if (answerEntity is null) 
            {
                return null;
            }
            return _mapper.Map<AnswerModel>(answerEntity);
        }
    }
}
