using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public FeedbackRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<bool> CreateAsync(FeedBack feedback)
        {
            await _context.FeedBacks.AddAsync(feedback);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<FeedBackModel>> GetAllMentorFeedback(Guid mentorId)
        {
            var feedbacks = await _context.FeedBacks.Where(x => x.StaffId == mentorId.ToString())
                                              .Select(x => new FeedBack { 
                                                    FeedbackId = x.FeedbackId,
                                                    Content = x.Content, 
                                                    RatingStar = x.RatingStar,
                                                    Member = x.Member
                                              }).ToListAsync();
            return _mapper.Map<IEnumerable<FeedBackModel>>(feedbacks);
        }

        public async Task<IEnumerable<FeedBackModel>> GetAllCourseFeedback(Guid courseId)
        {
            var feedbacks = await _context.FeedBacks.Where(x => x.CourseId == courseId.ToString())
                                              .Select(x => new FeedBack
                                              {
                                                  FeedbackId = x.FeedbackId,
                                                  Content = x.Content,
                                                  RatingStar = x.RatingStar,
                                                  Member = x.Member
                                              }).ToListAsync();
            return _mapper.Map<IEnumerable<FeedBackModel>>(feedbacks);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var feedbackEntity = await _context.FeedBacks.Where(x => x.FeedbackId == id)
                                                         .FirstOrDefaultAsync();

            if(feedbackEntity is not null)
            {
                _context.FeedBacks.Remove(feedbackEntity);
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }

            return false;
        }
    }
}
