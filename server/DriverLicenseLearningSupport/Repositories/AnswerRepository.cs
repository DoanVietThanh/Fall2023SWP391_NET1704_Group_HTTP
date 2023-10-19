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

        public async Task<AnswerModel> GetByQuestionIdAndAnswerDesc(int questionId, string answerDesc)
        {
            var question = await _context.Questions.Where(x => x.QuestionId == questionId)
                .Select(x => new Question {
                QuestionId = questionId,
                QuestionAnswers = x.QuestionAnswers.Select(x => new QuestionAnswer { 
                    QuestionAnswerId = x.QuestionAnswerId,
                    Answer = x.Answer,
                    IsTrue = x.IsTrue,
                    QuestionId = x.QuestionId
                }).ToList()
            }).FirstOrDefaultAsync();


            var answerEntity = question.QuestionAnswers.Where(x => x.Answer.Equals(answerDesc)).FirstOrDefault();

            if (answerEntity is null)
            {
                return null;
            }
            return _mapper.Map<AnswerModel>(answerEntity);
        }

        public async Task<int> GetRightAnswerIdByQuestionId(int questionId)
        {
            List<QuestionAnswer> answerEntities = await _context.QuestionAnswers.Where(a => a.QuestionId == questionId)
                .ToListAsync();
            foreach(QuestionAnswer answer in answerEntities) 
            {
                if (answer.IsTrue == true) 
                {
                    return answer.QuestionAnswerId;
                }
            }
            return -1;
        }

        public async Task<AnswerModel> UpdateAnswerAsync(int answerId, AnswerModel answer)
        {
            var answerEntity = await _context.QuestionAnswers.Where(a => a.QuestionAnswerId == answerId)
                .FirstOrDefaultAsync();
            answerEntity.Answer = answer.Answer;
            answerEntity.IsTrue = answer.IsTrue;
            await _context.SaveChangesAsync();
            return _mapper.Map<AnswerModel>(answerEntity);
        }
    }
}
