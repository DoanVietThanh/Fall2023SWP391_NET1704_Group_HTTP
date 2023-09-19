using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IAddressRepository
    {
        Task<bool> CreateAsync(Address address);
        Task<AddressModel> FindByIdAsync(string id);
    }
}
