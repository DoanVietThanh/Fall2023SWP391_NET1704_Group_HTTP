using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository,
            IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(AddressModel address)
        {
           var addressEntity = _mapper.Map<Address>(address);
           return await _addressRepository.CreateAsync(addressEntity);
        }

        public async Task<AddressModel> FindByIdAsync(Guid id)
        {
            return await _addressRepository.FindByIdAsync(id.ToString());
        }
    }
}
