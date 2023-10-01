using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IPaymentTypeService
    {
        Task<IEnumerable<PaymentTypeModel>> GetAllAsync();
    }
}
