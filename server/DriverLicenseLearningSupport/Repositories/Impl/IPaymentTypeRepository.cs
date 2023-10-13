using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IPaymentTypeRepository
    {
        Task<PaymentTypeModel> GetAsync(int id);
        Task<IEnumerable<PaymentTypeModel>> GetAllAsync();
    }
}
