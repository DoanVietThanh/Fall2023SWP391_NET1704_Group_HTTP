using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IMapper _mapper;
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IMapper mapper, IAnswerRepository answerRepository)
        {
            _mapper = mapper;
            _answerRepository = answerRepository;
        }
        public async Task<AnswerModel> CreateAsync(AnswerModel answer)
        {
            var answerEntity = _mapper.Map<QuestionAnswer>(answer);
            return await _answerRepository.CreateAsync(answerEntity);
        }

        public Task<IEnumerable<AnswerModel>> CreateRangeAsync(IEnumerable<AnswerModel> answers)
        {
            var answerEntities = _mapper.Map<IEnumerable<QuestionAnswer>>(answers);
            return _answerRepository.CreateRangeAsync(answerEntities);
        }

        public async Task<bool> DeleteAnswerAsync(int answerId)
        {
            return await _answerRepository.DeleteAnswerAsync(answerId);
        }

        public async Task<bool> DeleteAnswersByQuestionIdAsync(int quesitonId)
        {
            return await _answerRepository.DeleteAnswersByQuestionIdAsync(quesitonId);
        }

        public async Task<IEnumerable<AnswerModel>> GetAllByQuestionId(int questionId)
        {
            var answers = await _answerRepository.GetAllByQuestionId(questionId);

            var listAnswers = answers.ToList();
            var index = 0;
            while(index < answers.Count())
            {
                listAnswers[index].QuestionAnswerId = index;
                ++index;
            }
            return listAnswers;
        }

        public async Task<AnswerModel> GetByAnswerIdAsync(int answerId)
        {
            return await _answerRepository.GetByAnswerIdAsync(answerId);
        }

        public async Task<AnswerModel> GetByQuestionIdAndAnswerDesc(int questionId, string answerDesc)
        {
            return await _answerRepository.GetByQuestionIdAndAnswerDesc(questionId, answerDesc);
        }

        public async Task<int> GetRightAnswerIdByQuestionId(int questionId)
        {
            return await _answerRepository.GetRightAnswerIdByQuestionId(questionId);
        }
    }
}
