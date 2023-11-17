using AutoMapper;
using AutoMapper.Execution;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Repositories.Impl;
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
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public StaffService(IStaffRepository staffRepository,
            ILicenseTypeService licenseTypeService,
            IAddressService addressService,
            IJobTitleService jobTitleService,
            IImageService imageService,
            IMapper mapper)
        {
            _staffRepository = staffRepository;
            _licenseTypeService = licenseTypeService;
            _addressService = addressService;
            _jobTitleService = jobTitleService;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(StaffModel model)
        {
            var staffEntity = _mapper.Map<Staff>(model);
            return await _staffRepository.CreateAsync(staffEntity);
        }

        public async Task<StaffModel> GetByEmailAsync(string email)
        {
            var staff = await _staffRepository.GetByEmailAsync(email);
            if (staff is not null)
            {
                staff.AvatarImage = await _imageService.GetPreSignedURL(Guid.Parse(staff.AvatarImage));
            }
            return staff;
        }

        public async Task<StaffModel> GetAsync(Guid id)
        {
            var staff = await _staffRepository.GetAsync(id);
            if (staff.AvatarImage is not null)
            {
                staff.AvatarImage = await _imageService.GetPreSignedURL(Guid.Parse(staff.AvatarImage));
            }
            return staff;
        }

        public async Task<StaffModel> GetMentorAsync(Guid id)
        {
            return await _staffRepository.GetMentorAsync(id);
        }
        
        public async Task<IEnumerable<StaffModel>> GetAllAsync()
        {
            var staffs = await _staffRepository.GetAllAsync();

            if (staffs.Count() > 0)
            {
                foreach (var staff in staffs)
                {
                    if (staff.AvatarImage is not null)
                    {
                        staff.AvatarImage = await _imageService.GetPreSignedURL(Guid.Parse(staff.AvatarImage));
                    }
                }
            }
            return staffs;
        }

        public async Task<IEnumerable<StaffModel>> GetAllByFilterAsync(StaffFilter filters)
        {
            return await _staffRepository.GetAllByFilterAsync(filters);
        }

        public async Task<bool> UpdateAsync(Guid id, StaffModel staff)
        {
            var staffEntity = _mapper.Map<Staff>(staff);
            return await _staffRepository.UpdateAsync(id, staffEntity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _staffRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<StaffModel>> GetAllMentorAsync()
        {
            var staffs = await _staffRepository.GetAllMentorAsync();

            if (staffs.Count() > 0)
            {
                foreach (var staff in staffs)
                {
                    if (staff.AvatarImage is not null)
                    {
                        staff.AvatarImage = await _imageService.GetPreSignedURL(Guid.Parse(staff.AvatarImage));
                    }
                }
            }
            return staffs;
        }

        public async Task<IEnumerable<StaffModel>> GetAllMentorNoCourse()
        {
            return await _staffRepository.GetAllMentorNoCourse();
        }
    }
}
