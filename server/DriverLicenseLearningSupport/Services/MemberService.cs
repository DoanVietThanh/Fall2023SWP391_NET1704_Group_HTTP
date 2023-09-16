using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class MemberService : IMemberService
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public MemberService(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(MemberModel member)
        {
            var memberEntity = _mapper.Map<Member>(member);
            await _context.Members.AddAsync(memberEntity);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
