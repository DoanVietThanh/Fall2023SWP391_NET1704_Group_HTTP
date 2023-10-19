using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleModel>> GetAllAsync()
        {
            var roleEntities = await _context.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleModel>>(roleEntities);
        }

        public async Task<RoleModel> GetAsync(int id)
        {
            var roleEntity = await _context.Roles.Where(x => x.RoleId == id)
                                                 .FirstOrDefaultAsync();
            return _mapper.Map<RoleModel>(roleEntity);
        }

        public async Task<RoleModel> GetByNameAsync(string name)
        {
            var roleEntity = await _context.Roles.Where(x => x.Name == name)
                                                 .FirstOrDefaultAsync();
            return _mapper.Map<RoleModel>(roleEntity);
        }

        public async Task<RoleModel> GetMemberRoleIdAsync()
        {
            var roleEntity = await _context.Roles.Where(x => x.Name.Equals("Member"))
                                                  .FirstOrDefaultAsync();
            return _mapper.Map<RoleModel>(roleEntity);
        }
    }
}
