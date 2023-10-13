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

        public TheoryExamService(ITheoryExamRepository theoryExamRepository,
            IMapper mapper) 
        {
            _theoryExamRepository= theoryExamRepository;
            _mapper = mapper;
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
            }

            return theoryExamModel;
        }

        public async Task<IEnumerable<TheoryExamModel>> GetByLicenseTypeIdAsync(int licenseTypeId)
        {
            return await _theoryExamRepository.GetByLicenseTypeIdAsync(licenseTypeId);
        }

        public async Task<bool> IsExamQuestion(int questionId)
        {
            return await _theoryExamRepository.IsExamQuestion(questionId);
        }
    }
}
