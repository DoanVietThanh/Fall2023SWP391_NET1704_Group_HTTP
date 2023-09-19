using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleModel>> FindAllAsync();
    }
}
