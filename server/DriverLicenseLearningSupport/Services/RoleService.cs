using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<IEnumerable<RoleModel>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<RoleModel> GetByNameAsync(string name)
        {
            return await _roleRepository.GetByNameAsync(name);
        }

        public async Task<RoleModel> GetMemberRoleIdAsync()
        {
            return await _roleRepository.GetMemberRoleIdAsync();
        }
    }
}
