using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class LicenseRegisterFormService : ILicenseRegisterFormService
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly ILicenseRegisterFormRepository _licenseRegisterFormRepo;

        public LicenseRegisterFormService(ILicenseRegisterFormRepository licenseRegisterFormRepo,
            IImageService imageService,
            IMapper mapper)
        {
            _mapper = mapper;
            _imageService = imageService;
            _licenseRegisterFormRepo = licenseRegisterFormRepo;
        }

        public async Task<bool> ApproveAsync(int licenseRegisterId)
        {
            return await _licenseRegisterFormRepo.ApproveAsync(licenseRegisterId);
        }

        public async Task<LicenseRegisterFormModel> CreateAsync(LicenseRegisterFormModel model, Guid memberId)
        {
            var entity = _mapper.Map<LicenseRegisterForm>(model);
            return await _licenseRegisterFormRepo.CreateAsync(entity, memberId);
        }

        public async Task<IEnumerable<LicenseRegisterFormModel>> GetAllAwaitAsync()
        {
            return await _licenseRegisterFormRepo.GetAllAwaitAsync();
        }

        public async Task<LicenseRegisterFormModel> GetAsync(int licenseRegisterId)
        {
            return await _licenseRegisterFormRepo.GetAsync(licenseRegisterId);
        }

        public async Task<LicenseRegisterFormModel> GetByMemberId(Guid memberId)
        {
            return await _licenseRegisterFormRepo.GetByMemberId(memberId);
        }
    }
}
