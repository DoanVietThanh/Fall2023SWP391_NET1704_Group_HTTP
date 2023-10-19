using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Services
{
    public class LicenseTypeService : ILicenseTypeService
    {
        private readonly ILicenseTypeRepository _licenseTypeRepository;

        public LicenseTypeService(ILicenseTypeRepository licenseTypeRepository)
        {
            _licenseTypeRepository = licenseTypeRepository;
        }
        public async Task<IEnumerable<LicenseTypeModel>> GetAllAsync()
        {
            return await _licenseTypeRepository.GetAllAsync();
        }

        public async Task<LicenseTypeModel> GetAsync(int id)
        {
            return await _licenseTypeRepository.GetAsync(id);
        }

        public async Task<LicenseTypeModel> GetByDescAsync(string licenseTypeDesc)
        {
            return await _licenseTypeRepository.GetByDescAsync(licenseTypeDesc);
        }
    }
}
