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
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public MemberService(IMemberRepository memberRepository, 
            IAddressService addressService,
            ILicenseTypeService licenseTypeService,
            IAccountService accountService,
            IMapper mapper)
        {
            _memberRepository = memberRepository;
            _addressService = addressService;
            _licenseTypeService = licenseTypeService;
            _accountService = accountService; 
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
        public async Task<MemberModel> GetByEmailAsync(string email)
        {
            var member = await _memberRepository.GetByEmailAsync(email);
            if (member is not null)
            {
                member.LicenseType = await _licenseTypeService.GetAsync(Convert.ToInt32(member.LicenseTypeId));
                member.Address = await _addressService.GetAsync(Guid.Parse(member.AddressId));
                member.EmailNavigation = await _accountService.GetByEmailAsync(member.Email);
                member.EmailNavigation.Password = null!;
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

    }
}
