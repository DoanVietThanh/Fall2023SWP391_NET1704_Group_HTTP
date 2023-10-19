using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IPaymentTypeService
    {
        Task<PaymentTypeModel> GetAsync(int id);
        Task<IEnumerable<PaymentTypeModel>> GetAllAsync();
    }
}
