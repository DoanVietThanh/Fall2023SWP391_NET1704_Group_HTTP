using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IMapper mapper, IQuestionRepository questionRepository)
        {
            _mapper = mapper;
            _questionRepository = questionRepository;
        }

        public async Task<QuestionModel> CreateAsync(QuestionModel question)
        {
            var questionEntity = _mapper.Map<Question>(question);
            return await _questionRepository.CreateAsync(questionEntity);
        }

        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            return await _questionRepository.DeleteQuestionAsync(questionId);
        }

        public async Task<IEnumerable<QuestionModel>> GetAllAsync()
        {
            return await _questionRepository.GetAllAsync();
        }
        public async Task<QuestionModel> GetByIdAsync(int questionId) 
        {
            return await _questionRepository.GetByIdAsync(questionId);
        }
    }
}
