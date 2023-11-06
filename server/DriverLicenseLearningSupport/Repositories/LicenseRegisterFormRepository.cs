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
            if (lfEntity is not null)
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
            // get member by id 
            var member = await _context.Members.Where(x => x.MemberId == memberId.ToString())
                                               .FirstOrDefaultAsync();


            var lfEntity = await _context.LicenseRegisterForms.Where(x => x.LicenseFormId == member.LicenseFormId)
                                                             .FirstOrDefaultAsync();

            return _mapper.Map<LicenseRegisterFormModel>(lfEntity);
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

        public async Task<IEnumerable<LicenseRegisterFormModel>> GetAllAwaitAsync()
        {
            var awaitForms = await _context.LicenseRegisterForms.Where(x => x.RegisterFormStatusId == 1)
                                                                .ToListAsync();

            return _mapper.Map<IEnumerable<LicenseRegisterFormModel>>(awaitForms);
        }

        public async Task<bool> DenyAsync(int licenseRegisterId)
        {
            // get by id
            var lfRegisterEntity = await _context.LicenseRegisterForms.Where(x => x.LicenseFormId == licenseRegisterId)
                                                                .FirstOrDefaultAsync();
            // not found
            if (lfRegisterEntity is null) return false;

            // approve <- change status
            lfRegisterEntity.RegisterFormStatusId = 3;
            // save changes
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(int licenseRegisterId, LicenseRegisterFormModel model)
        {
            // get by id
            var lfRegisterEntity = await _context.LicenseRegisterForms.Where(x => x.LicenseFormId == licenseRegisterId)
                                                                .FirstOrDefaultAsync();
            // not found
            if (lfRegisterEntity is null) return false;

            // update fields
            lfRegisterEntity.LicenseTypeId = model.LicenseTypeId;
            lfRegisterEntity.PermanentAddress = model.PermanentAddress;
            lfRegisterEntity.IdentityNumber = model.IdentityNumber;
            lfRegisterEntity.Gender = model.Gender;
            lfRegisterEntity.IdentityCardIssuedDate = model.IdentityCardIssuedDate;
            lfRegisterEntity.IdentityCardIssuedBy = model.IdentityCardIssuedBy;
            lfRegisterEntity.AvailableLicenseType = model.AvailableLicenseType;
            
            if(model.Image is not null) lfRegisterEntity.Image = model.Image;
            if(model.IdentityCardImage is not null)
                lfRegisterEntity.IdentityCardImage = model.IdentityCardImage;
            if(model.HealthCertificationImage is not null)
                lfRegisterEntity.HealthCertificationImage = model.HealthCertificationImage;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
