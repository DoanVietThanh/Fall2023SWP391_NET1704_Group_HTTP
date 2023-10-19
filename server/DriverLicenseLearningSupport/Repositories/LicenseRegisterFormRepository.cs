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

        public async Task<LicenseRegisterFormModel> GetAsync(int licenseRegisterId)
        {
            var lfEntity = await _context.LicenseRegisterForms.Where(x => x.LicenseFormId == licenseRegisterId)
                                    .FirstOrDefaultAsync();  
            // get form status
            if(lfEntity is not null)
            {
                LicenseRegisterFormStatus? licenseRegisterFormStatus = await _context.LicenseRegisterFormStatuses
                                                                            .Where(x => x.RegisterFormStatusId == lfEntity.RegisterFormStatusId)
                                                                            .FirstOrDefaultAsync();
                lfEntity.RegisterFormStatus = licenseRegisterFormStatus;
            }
            return _mapper.Map<LicenseRegisterFormModel>(lfEntity);
        }
        public async Task<LicenseRegisterFormModel> GetByMemberId(Guid memberId)
        {
            var lfEntities = await _context.LicenseRegisterForms
                                         .Select(x => new LicenseRegisterForm { 
                                            Members = x.Members.Select(x => new Member { 
                                                MemberId = x.MemberId
                                            }).ToList()
                                         }).ToListAsync();
            var memberEntity = lfEntities.Select(x => x.Members.Where(x => x.MemberId == memberId.ToString())
                                                               .FirstOrDefault())
                                         .SingleOrDefault();
            if(memberEntity is not null)
            {
                var lfEntity = await _context.LicenseRegisterForms.Where(x => x.LicenseFormId == memberEntity.LicenseFormId)
                                    .FirstOrDefaultAsync();         
                return _mapper.Map<LicenseRegisterFormModel>(lfEntity);
            }
            return null;
        }
        public async Task<bool> ApproveAsync(int licenseRegisterId)
        {
            // get by id
            var lfRegisterEntity = await _context.LicenseRegisterForms.Where(x => x.LicenseFormId == licenseRegisterId)
                                                                .FirstOrDefaultAsync();
            // not found
            if (lfRegisterEntity is null) return false;

            // approve <- change status
            lfRegisterEntity.RegisterFormStatusId = 2;
            // save changes
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

    }
}
