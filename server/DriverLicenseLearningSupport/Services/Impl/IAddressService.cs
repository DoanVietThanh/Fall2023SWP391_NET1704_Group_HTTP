using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IAddressService
    {
        Task<bool> CreateAsync(AddressModel address);
    }
}
