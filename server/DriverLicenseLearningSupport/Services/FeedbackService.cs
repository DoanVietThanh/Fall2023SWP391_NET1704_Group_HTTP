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
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepo,
            IImageService imageService,
            IMapper mapper)
        {
            _feedbackRepo = feedbackRepo;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(FeedBackModel feedback)
        {
            var feedbackEntity = _mapper.Map<FeedBack>(feedback);
            return await _feedbackRepo.CreateAsync(feedbackEntity);
        }

        public async Task<IEnumerable<FeedBackModel>> GetAllCourseFeedback(Guid courseId)
        {
            var feedbacks = await _feedbackRepo.GetAllCourseFeedback(courseId);
            foreach (var feedback in feedbacks)
            {
                var imageId = feedback.Member.AvatarImage;
                feedback.Member.AvatarImage = await _imageService.GetPreSignedURL(Guid.Parse(imageId));
            }
            return feedbacks;
        }

        public async Task<IEnumerable<FeedBackModel>> GetAllMentorFeedback(Guid mentorId)
        {
            var feedbacks = await _feedbackRepo.GetAllMentorFeedback(mentorId);
            foreach(var feedback in feedbacks) 
            {
                var imageId = feedback.Member.AvatarImage;
                feedback.Member.AvatarImage = await _imageService.GetPreSignedURL(Guid.Parse(imageId));
            }
            return feedbacks;
        }


    }
}
