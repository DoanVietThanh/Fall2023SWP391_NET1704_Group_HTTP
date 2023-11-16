using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class TheoryExamService : ITheoryExamService
    {
        private readonly ITheoryExamRepository _theoryExamRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public TheoryExamService(ITheoryExamRepository theoryExamRepository,
            IMapper mapper,
            IImageService imageService) 
        {
            _theoryExamRepository= theoryExamRepository;
            _mapper = mapper;
            _imageService = imageService; 
        }

        public async Task<bool> AddQuestionAsync(int theoryExamId, int quesitonId)
        {
            return await _theoryExamRepository.AddQuesitonAsync(theoryExamId, quesitonId);
        }

        public async Task<TheoryExamModel> CreateAsync(TheoryExamModel theoryExam)
        {
            var theoryExamEntity = _mapper.Map<TheoryExam>(theoryExam);
            return await _theoryExamRepository.CreateAsync(theoryExamEntity);
        }

        public async Task<IEnumerable<TheoryExamModel>> GetAllAsync()
        {
            return await _theoryExamRepository.GetAllAsync();
        }

        public async Task<IEnumerable<TheoryExamModel>> GetAllMockTest()
        {
            return await _theoryExamRepository.GetAllMockTest();
        }

        public async Task<TheoryExamModel> GetByIdAsync(int id)
        {
            var theoryExamModel =  await _theoryExamRepository.GetByIdAsync(id);
            if(theoryExamModel is null) 
            {
                return null;
            }

            // loop all question
            foreach(var question in theoryExamModel.Questions)
            {
                var answers = question.QuestionAnswers.ToList();
                var countAnswer = answers.Count;
                var index = 0;
                // convert to format ids
                while(index < countAnswer)
                {
                    answers[index].QuestionAnswerId = index;
                    ++index;
                }
                // set answers list
                question.QuestionAnswers = answers;

                if(question.Image is not null)
                {
                    question.Image = await _imageService.GetPreSignedURL(Guid.Parse(question.Image));
                }
            }

            return theoryExamModel;
        }

        public async Task<IEnumerable<TheoryExamModel>> GetByLicenseTypeIdAsync(int licenseTypeId)
        {
            return await _theoryExamRepository.GetByLicenseTypeIdAsync(licenseTypeId);
        }

        public async Task<bool> HasHistory(int id)
        {
            return await _theoryExamRepository.HasHistory(id);
        }

        public async Task<bool> IsExamQuestion(int questionId)
        {
            return await _theoryExamRepository.IsExamQuestion(questionId);
        }

        public async Task<bool> RemoveTheoryExam(int id)
        {
            return await _theoryExamRepository.RemoveTheoryExam(id);
        }
    }
}
