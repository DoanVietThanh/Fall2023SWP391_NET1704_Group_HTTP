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
        public async Task<LicenseRegisterFormModel> CreateAsync(LicenseRegisterFormModel model, Guid memberId)
        {
            var entity = _mapper.Map<LicenseRegisterForm>(model);
            return await _licenseRegisterFormRepo.CreateAsync(entity, memberId);
        }
    }
}
