using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IPaymentTypeRepository _paymentTypeRepo;

        public PaymentTypeService(IPaymentTypeRepository paymentTypeRepo)
        {
            _paymentTypeRepo = paymentTypeRepo;
        }

        public async Task<IEnumerable<PaymentTypeModel>> GetAllAsync()
        {
            return await _paymentTypeRepo.GetAllAsync();
        }
    }
}
