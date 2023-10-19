using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IRoleRepository
    {
        Task<RoleModel> GetAsync(int id);
        Task<IEnumerable<RoleModel>> GetAllAsync();
        Task<RoleModel> GetMemberRoleIdAsync();
        Task<RoleModel> GetByNameAsync(string name);
    }
}
