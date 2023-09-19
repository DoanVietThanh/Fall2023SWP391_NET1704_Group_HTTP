using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public JobTitleRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobTitleModel>> FindAllAsync()
        {
            var JobTitleEntities = await _context.JobTitles.ToListAsync();
            return _mapper.Map<IEnumerable<JobTitleModel>>(JobTitleEntities);
        }

        public async Task<JobTitleModel> FindByIdAsync(int id)
        {
            var jobTitleEntity = await _context.JobTitles.Where(x => x.JobTitleId == id)
                                                         .FirstOrDefaultAsync();
            return _mapper.Map<JobTitleModel>(jobTitleEntity);
        }
    }
}
