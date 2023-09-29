using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepo;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepo,
            IMapper mapper)
        {
            _feedbackRepo = feedbackRepo;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(FeedBackModel feedback)
        {
            var feedbackEntity = _mapper.Map<FeedBack>(feedback);
            return await _feedbackRepo.CreateAsync(feedbackEntity);
        }

        public async Task<IEnumerable<FeedBackModel>> GetAllMentorFeedback(Guid mentorId)
        {
            return await _feedbackRepo.GetAllMentorFeedback(mentorId);
        }
    }
}
