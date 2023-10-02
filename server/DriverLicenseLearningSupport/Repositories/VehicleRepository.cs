using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public VehicleRepository(DriverLicenseLearningSupportContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VehicleModel> CreateAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (!isSuccess) return null;
            return _mapper.Map<VehicleModel>(vehicle);
        }

        public async Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAsync()
        {
            var vehicleTypes = await _context.VehicleTypes.ToListAsync();
            return _mapper.Map<IEnumerable<VehicleTypeModel>>(vehicleTypes);
        }
    }
}
