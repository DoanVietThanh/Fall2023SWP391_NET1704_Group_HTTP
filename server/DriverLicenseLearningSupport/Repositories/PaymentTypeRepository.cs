using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class PaymentTypeRepository : IPaymentTypeRepository
    {
        private readonly IMapper _mapper;
        private readonly DriverLicenseLearningSupportContext _context;

        public PaymentTypeRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IEnumerable<PaymentTypeModel>> GetAllAsync()
        {
            var paymentTypes = await _context.PaymentTypes.ToListAsync();
            return _mapper.Map<IEnumerable<PaymentTypeModel>>(paymentTypes);
        }

        public async Task<PaymentTypeModel> GetAsync(int id)
        {
            var paymentType = await _context.PaymentTypes.Where(x => x.PaymentTypeId == id)
                                                    .FirstOrDefaultAsync();
            return _mapper.Map<PaymentTypeModel>(paymentType);
        }
    }
}
