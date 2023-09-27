using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class JobTitleService : IJobTitleService
    {
        private readonly IJobTitleRepository _jobTitleRepository;

        public JobTitleService(IJobTitleRepository jobTitleRepository)
        {
            _jobTitleRepository = jobTitleRepository;
        }

        public async Task<IEnumerable<JobTitleModel>> GetAllAsync()
        {
            return await _jobTitleRepository.GetAllAsync();
        }

        public async Task<JobTitleModel> GetAsync(int id)
        {
            return await _jobTitleRepository.GetAsync(id);
        }

        public async Task<JobTitleModel> GetByDescAsync(string desc)
        {
            return await _jobTitleRepository.GetByDescAsync(desc);
        }
    }
}
