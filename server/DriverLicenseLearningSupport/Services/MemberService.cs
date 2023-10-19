using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class MemberService : IMemberService
    {
        private readonly IAddressService _addressService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IAccountService _accountService;
        private readonly IImageService _imageService;
        private readonly ILicenseRegisterFormService _licenseRegisterFormService;
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public MemberService(IMemberRepository memberRepository, 
            IAddressService addressService,
            ILicenseTypeService licenseTypeService,
            IAccountService accountService,
            IImageService imageService,
            ILicenseRegisterFormService licenseRegisterFormService,
            IMapper mapper)
        {
            _memberRepository = memberRepository;
            _addressService = addressService;
            _licenseTypeService = licenseTypeService;
            _accountService = accountService;
            _imageService = imageService;
            _licenseRegisterFormService = licenseRegisterFormService;
            _mapper = mapper;
        }

        public async Task<MemberModel> CreateAsync(MemberModel member)
        {
            var memberEntity = _mapper.Map<Member>(member);
            return await _memberRepository.CreateAsync(memberEntity);
        }
        public async Task<IEnumerable<MemberModel>> CreateRangeAsync(IEnumerable<MemberModel> members)
        {
            var memberEntities = _mapper.Map<IEnumerable<Member>>(members);
            return await _memberRepository.CreateRangeAsync(memberEntities);
        }
        public async Task<LicenseRegisterFormModel> CreateLicenseRegisterFormAsync(LicenseRegisterFormModel licenseRegisterFormModel, Guid memberId)
        {
            return await _licenseRegisterFormService.CreateAsync(licenseRegisterFormModel, memberId);
        }
        public async Task<MemberModel> GetByEmailAsync(string email)
        {
            var member = await _memberRepository.GetByEmailAsync(email);
            if (member is not null)
            {
                //member.AvatarImage = await _imageService.GetPreSignedURL(Guid.Parse(member.AvatarImage));
                member.LicenseForm = await _licenseRegisterFormService.GetAsync(Convert.ToInt32(member.LicenseFormId));
                if (member.LicenseForm is not null)
                {
                    //member.LicenseForm.Image 
                    //    = await _imageService.GetPreSignedURL(Guid.Parse(member.LicenseForm.Image));
                    //member.LicenseForm.IdentityCardImage 
                    //    = await _imageService.GetPreSignedURL(Guid.Parse(member.LicenseForm.IdentityCardImage));
                    //member.LicenseForm.HealthCertificationImage 
                    //    = await _imageService.GetPreSignedURL(Guid.Parse(member.LicenseForm.HealthCertificationImage));
                }
            }
            return member;
        }
        public async Task<MemberModel> GetAsync(Guid id)
        {
            var member = await _memberRepository.GetAsync(id);
            if(member is not null ) 
            {
                member.LicenseType = await _licenseTypeService.GetAsync(Convert.ToInt32(member.LicenseTypeId));
                member.Address = await _addressService.GetAsync(Guid.Parse(member.AddressId));
                member.EmailNavigation = await _accountService.GetByEmailAsync(member.Email);
                member.EmailNavigation.Password = null!;
                member.AvatarImage = await _imageService.GetPreSignedURL(Guid.Parse(member.AvatarImage));
            }
            return member;
        }
        public async Task<IEnumerable<MemberModel>> GetAllAsync()
        {
            return await _memberRepository.GetAllAsync();
        }
        public async Task<IEnumerable<MemberModel>> GetAllByFilterAsync(MemberFilter filters)
        {
            return await _memberRepository.GetAllAsyncByFilter(filters);
        }
        public async Task<bool> UpdateAsync(Guid id, MemberModel member)
        {
            return await _memberRepository.UpdateAsync(id, member);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _memberRepository.DeleteAsync(id);
        }
        public async Task<bool> HideMemberAsync(Guid id)
        {
            return await _memberRepository.HideMemberAsync(id);
        }
        public async Task<MemberModel> GetByLicenseRegisterFormIdAsync(int licenseRegisterFormId)
        {
            return await _memberRepository.GetByLicenseRegisterFormIdAsync(licenseRegisterFormId);
        }
    }
}
