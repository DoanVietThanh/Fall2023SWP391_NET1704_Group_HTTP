using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IPaymentTypeRepository
    {
        Task<IEnumerable<PaymentTypeModel>> GetAllAsync();
    }
}
