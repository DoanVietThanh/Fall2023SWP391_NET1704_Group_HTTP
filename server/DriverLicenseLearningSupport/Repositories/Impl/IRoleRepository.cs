using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IRoleRepository
    {
        Task<RoleModel> FindByIdAsync(int id);
        Task<IEnumerable<RoleModel>> FindAllAsync();

    }
}
