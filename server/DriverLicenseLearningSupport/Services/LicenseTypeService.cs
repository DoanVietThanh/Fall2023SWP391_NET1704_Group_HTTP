using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.impl;
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
        public async Task<IEnumerable<LicenseTypeModel>> FindAllAsync()
        {
            return await _licenseTypeRepository.FindAllAsync();
        }

        public async Task<LicenseTypeModel> FindByIdAsync(int id)
        {
            return await _licenseTypeRepository.FindByIdAsync(id);
        }
    }
}
