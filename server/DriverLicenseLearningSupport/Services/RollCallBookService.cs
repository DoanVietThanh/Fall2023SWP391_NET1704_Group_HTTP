using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class RollCallBookService : IRollCallBookService
    {
        private readonly IRollCallBookRepository _rcbRepo;

        public RollCallBookService(IRollCallBookRepository rcbRepo)
        {
            _rcbRepo = rcbRepo;    
        }
        public async Task<IEnumerable<RollCallBookModel>> GetAllByMemberIdAsync(Guid memberId)
        {
            return await _rcbRepo.GetAllByMemberIdAsync(memberId);
        }

        public async Task<RollCallBookModel> GetAsync(int id)
        {
            return await _rcbRepo.GetAsync(id);
        }

        public async Task<bool> UpdateAsync(int rcbId, RollCallBookModel rcbook)
        {
            return await _rcbRepo.UpdateAsync(rcbId, rcbook);
        }
    }
}
