using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;
        public MemberRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(Member member)
        {
            await _context.Members.AddAsync(member);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<MemberModel> FindByEmailAsync(string email)
        {
            var memberEntity = await _context.Members.Where(x => x.Email.Equals(email))
                                                     .FirstOrDefaultAsync();
            return _mapper.Map<MemberModel>(memberEntity);
        }

        public async Task<MemberModel> FindByIdAsync(Guid id)
        {
            var memberEntity = await _context.Members.Where(x => x.MemberId == id.ToString())
                                                     .FirstOrDefaultAsync();
            return _mapper.Map<MemberModel>(memberEntity);
        }
    }
}
