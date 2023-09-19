using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IAddressService _addressService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IMapper _mapper;

        public MemberService(IMemberRepository memberRepository, 
            IAddressService addressService,
            ILicenseTypeService licenseTypeService,
            IMapper mapper)
        {
            _memberRepository = memberRepository;
            _addressService = addressService;
            _licenseTypeService = licenseTypeService;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(MemberModel member)
        {
            var memberEntity = _mapper.Map<Member>(member);
            return await _memberRepository.CreateAsync(memberEntity);
        }

        public async Task<MemberModel> FindByEmailAsync(string email)
        {
            var member = await _memberRepository.FindByEmailAsync(email);
            member.LicenseType = await _licenseTypeService.FindByIdAsync(Convert.ToInt32(member.LicenseTypeId));
            member.Address = await _addressService.FindByIdAsync(Guid.Parse(member?.AddressId));
            return member;
        }

        public async Task<MemberModel> FindByIdAsync(Guid id)
        {
            return await _memberRepository.FindByIdAsync(id);
        }
    }
}
