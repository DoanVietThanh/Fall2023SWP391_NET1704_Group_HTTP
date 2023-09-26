using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public StaffRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(Staff staff)
        {
            _context.Staffs.Add(staff);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<StaffModel> GetByEmailAsync(string email)
        {
            var staffEntity = await _context.Staffs.Where(x => x.Email == email)
                                                   .Select(x => new Staff
                                                   {
                                                       StaffId = x.StaffId,
                                                       FirstName = x.FirstName,
                                                       LastName = x.LastName,
                                                       DateBirth = x.DateBirth,
                                                       Email = x.Email,
                                                       AvatarImage = x.AvatarImage,
                                                       Phone = x.Phone,
                                                       Address = new Address
                                                       {
                                                           AddressId = x.AddressId,
                                                           Street = x.Address.Street,
                                                           District = x.Address.District,
                                                           City = x.Address.City,
                                                           Zipcode = x.Address.Zipcode,
                                                       },
                                                       LicenseType = new LicenseType
                                                       {
                                                           LicenseTypeId = x.LicenseType.LicenseTypeId,
                                                           LicenseTypeDesc = x.LicenseType.LicenseTypeDesc
                                                       },
                                                       JobTitle = new JobTitle
                                                       {
                                                           JobTitleId = x.JobTitle.JobTitleId,
                                                           JobTitleDesc = x.JobTitle.JobTitleDesc
                                                       }
                                                   }).FirstOrDefaultAsync();
            return _mapper.Map<StaffModel>(staffEntity);
        }

        public async Task<StaffModel> GetAsync(Guid id)
        {
            var staffEntity = await _context.Staffs.Where(x => x.StaffId == id.ToString())
                                                   .FirstOrDefaultAsync();
            if(staffEntity is not null) staffEntity.EmailNavigation.Password = null!;
            return _mapper.Map<StaffModel>(staffEntity);
        }
    }
}
