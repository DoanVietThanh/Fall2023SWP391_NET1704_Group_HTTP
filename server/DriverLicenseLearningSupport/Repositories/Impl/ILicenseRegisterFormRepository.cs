﻿using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ILicenseRegisterFormRepository
    {
        Task<LicenseRegisterFormModel> CreateAsync(LicenseRegisterForm licenseRegister, Guid memberId); 
        Task<LicenseRegisterFormModel> GetAsync(int licenseRegisterId);
        Task<LicenseRegisterFormModel> GetByMemberId(Guid memberId);
        Task<bool> ApproveAsync(int licenseRegisterId);
        Task<bool> DenyAsync(int licenseRegisterId);
        Task<bool> UpdateAsync(int licenseRegisterId, LicenseRegisterFormModel model);
        Task<IEnumerable<LicenseRegisterFormModel>> GetAllAwaitAsync();
    }
}
