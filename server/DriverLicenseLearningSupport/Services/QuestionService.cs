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
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IImageService _imageService;
        private readonly IAnswerService _answerService;

        public QuestionService(IMapper mapper, IQuestionRepository questionRepository
            ,ILicenseTypeService licenseTypeService , IImageService imageService, IAnswerService answerSerive)
        {
            _mapper = mapper;
            _questionRepository = questionRepository;
            _licenseTypeService = licenseTypeService;
            _imageService = imageService;
            _answerService = answerSerive;
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
            var question = await _questionRepository.GetByIdAsync(questionId);
            var answers = await _answerService.GetAllByQuestionId(questionId);
            if (question is not null && answers is not null)
            {
                question.LicenseType = await _licenseTypeService.GetAsync(question.LicenseTypeId);
                //question.Image = await _imageService.GetPreSignedURL(Guid.Parse(question.Image));
                question.QuestionAnswers = answers.ToList();
            }
            return question;
        }

        public async Task<IEnumerable<QuestionModel>> GetAllByLicenseId(int licenseId)
        {
            return await _questionRepository.GetAllByLicenseId(licenseId);
        }

        public async Task<QuestionModel> UpdateStatusQuestionAsync(int questionId, bool status)
        {
             return await _questionRepository.UpdateStatusQuestionAsync(questionId, status);
        }

        public async Task<bool> CheckExistedQuestion(string questionDesc, int lisenceId)
        {
            return await _questionRepository.CheckExistedQuestion(questionDesc, lisenceId);
        }

        public async Task<QuestionModel> UpdateQuestionAsync(QuestionModel updatedModel, int questionId)
        {
            return await _questionRepository.UpdateQuestionAsync(updatedModel, questionId);
        }
    }
}
