using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public AddressRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<AddressModel> FindByIdAsync(string id)
        {
            var addressEntity = await _context.Addresses.Where(x => x.AddressId == id)
                                                        .FirstOrDefaultAsync();
            return _mapper.Map<AddressModel>(addressEntity);
        }
    }
}
