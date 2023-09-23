using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class LicenseRegisterFormRepository : ILicenseRegisterFormRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public LicenseRegisterFormRepository(DriverLicenseLearningSupportContext context,
              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<LicenseRegisterFormModel> CreateAsync(LicenseRegisterForm licenseRegister, Guid memberId)
        {
            await _context.LicenseRegisterForms.AddAsync(licenseRegister);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSucess)
            {
                // get member by id
                var member = await _context.Members.Where(x => x.MemberId == memberId.ToString())
                                                    .FirstOrDefaultAsync();
                // get created register form
                var result = await _context.LicenseRegisterForms.OrderByDescending(x => x.LicenseFormId).FirstOrDefaultAsync();
                // add license form to member
                member.LicenseFormId = result.LicenseFormId;

                // save changes
                await _context.SaveChangesAsync();

                return _mapper.Map<LicenseRegisterFormModel>(licenseRegister);
            }
            return null;
        }
    }
}
