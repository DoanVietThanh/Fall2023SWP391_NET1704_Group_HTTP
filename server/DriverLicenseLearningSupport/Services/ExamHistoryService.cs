using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class ExamHistoryService : IExamHistoryService
    {
        private readonly IExamHistoryRepository _examHistoryRepository;
        private readonly IMapper _mapper;

        public ExamHistoryService(IExamHistoryRepository examHistoryRepository,IMapper mapper) 
        {
            _examHistoryRepository = examHistoryRepository;
            _mapper = mapper;
        }

        public async Task<ExamHistoryModel> CreateAsync(ExamHistoryModel model)
        {
            var ExamHistoryModel = _mapper.Map<ExamHistory>(model);
            return await _examHistoryRepository.CreateAsync(ExamHistoryModel);
        }

        public Task<IEnumerable<ExamHistoryModel>> GetAllByMemberIdAsysn(string memberId)
        {
            return _examHistoryRepository.GetAllByMemberIdAsysn(memberId);
        }

        public async Task<ExamHistoryModel> GetHistoryDetailAsync(string MemberId, int TheoryExamId, DateTime joinDate)
        {
            return await _examHistoryRepository.GetHistoryDetailAsync(MemberId, TheoryExamId, joinDate);
        }
    }
}
