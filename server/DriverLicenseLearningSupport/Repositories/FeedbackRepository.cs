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
                                              .ToListAsync();

            return _mapper.Map<IEnumerable<FeedBackModel>>(feedbacks);
        }
    }
}
