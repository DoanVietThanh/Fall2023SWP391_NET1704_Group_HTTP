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

        public async Task<IEnumerable<JobTitleModel>> FindAllAsync()
        {
            return await _jobTitleRepository.FindAllAsync();
        }

        public async Task<JobTitleModel> FindByIdAsync(int id)
        {
            return await _jobTitleRepository.FindByIdAsync(id);
        }
    }
}
