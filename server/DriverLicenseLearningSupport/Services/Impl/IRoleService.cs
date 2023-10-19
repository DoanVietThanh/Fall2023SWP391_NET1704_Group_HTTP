using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleModel>> GetAllAsync();
        Task<RoleModel> GetMemberRoleIdAsync();
        Task<RoleModel> GetByNameAsync(string name);
    }
}
