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
                                                   .FirstOrDefaultAsync();
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
