using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class AddressService : IAddressService
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public AddressService(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(AddressModel address)
        {
            var addressEntity = _mapper.Map<Address>(address);
            await _context.Addresses.AddAsync(addressEntity);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
