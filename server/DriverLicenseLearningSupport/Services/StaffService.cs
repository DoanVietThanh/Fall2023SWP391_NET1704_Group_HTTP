using AutoMapper;
using AutoMapper.Execution;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.impl;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IAddressService _addressService;
        private readonly IJobTitleService _jobTitleService;
        private readonly IMapper _mapper;

        public StaffService(IStaffRepository staffRepository,
            ILicenseTypeService licenseTypeService,
            IAddressService addressService,
            IJobTitleService jobTitleService,
            IMapper mapper)
        {
            _staffRepository = staffRepository;
            _licenseTypeService = licenseTypeService;
            _addressService = addressService;
            _jobTitleService = jobTitleService;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(StaffModel model)
        {
            var staffEntity = _mapper.Map<Staff>(model);
            return await _staffRepository.CreateAsync(staffEntity);
        }

        public async Task<StaffModel> FindByEmailAsync(string email)
        {
            var staff =  await _staffRepository.FindByEmailAsync(email);
            staff.LicenseType = await _licenseTypeService.FindByIdAsync(Convert.ToInt32(staff.LicenseTypeId));
            staff.Address = await _addressService.FindByIdAsync(Guid.Parse(staff.AddressId));
            staff.JobTitle = await _jobTitleService.FindByIdAsync(Convert.ToInt32(staff.JobTitleId));
            return staff;
        }

        public async Task<StaffModel> FindByIdAsync(Guid id)
        {
            return await _staffRepository.FindByIdAsync(id);
        }
    }
}
