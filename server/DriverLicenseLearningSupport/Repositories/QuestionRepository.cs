using Amazon.S3.Model.Internal.MarshallTransformations;
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

        public async Task<QuestionModel> CreateAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSucess)
            {
                var questionEntity = await _context.Questions.OrderByDescending(x => x.QuestionId)
                    .FirstOrDefaultAsync();
                question.QuestionId = Convert.ToInt32(questionEntity.QuestionId);
            }
            return _mapper.Map<QuestionModel>(question);
        }

        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            //find question to remove
            var question = await _context.Questions.Where(x => x.QuestionId == questionId).FirstOrDefaultAsync();
            //question does not exist
            if (question is null) return false;
            // question exists
            _context.Questions.Remove(question);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<QuestionModel>> GetAllAsync()
        {
            //Get all question
            var questions = await _context.Questions.ToListAsync();

            return _mapper.Map<IEnumerable<QuestionModel>>(questions);
        }

        public async Task<QuestionModel> GetByIdAsync(int questionId)
        {
            // get question by id
            var questionEntity = await _context.Questions.Where(x => x.QuestionId.Equals(questionId))
                .FirstOrDefaultAsync();
            if (questionEntity == null) 
            {
                return null;
            }
            return _mapper.Map<QuestionModel>(questionEntity);
        }
    }
}
