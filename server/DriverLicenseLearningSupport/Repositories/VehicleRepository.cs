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

        public async Task<IEnumerable<VehicleModel>> GetAllByVehicleTypeIdAsync(int vehicleTypeId)
        {
            var vehicles = await _context.Vehicles.Where(x => x.VehicleTypeId == vehicleTypeId).ToListAsync();
            return _mapper.Map<IEnumerable<VehicleModel>>(vehicles);
        }

        public async Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAsync()
        {
            var vehicleTypes = await _context.VehicleTypes.Select(x => new VehicleType { 
                VehicleTypeId = x.VehicleTypeId,
                LicenseTypeId = x.LicenseTypeId,
                LicenseType = x.LicenseType,
                VehicleTypeDesc = x.VehicleTypeDesc
            }).ToListAsync();
            return _mapper.Map<IEnumerable<VehicleTypeModel>>(vehicleTypes);
        }

        public async Task<VehicleModel> GetVehicleByVehicleTypeAsync(int vehicleTypeId)
        {
            var vehicles = await _context.Vehicles.Where(x => x.VehicleTypeId == vehicleTypeId
                                                        && x.IsActive == true).ToListAsync();
            if(vehicles.Count() > 0)
            {
                return _mapper.Map<VehicleModel>(vehicles.FirstOrDefault());
            }
            return null!;
        }

        public async Task<VehicleTypeModel> GetVehicleTypeByLicenseTypeAsync(int licenseTypeId)
        {
            var vehicleType = await _context.VehicleTypes.Where(x => x.LicenseTypeId == licenseTypeId)
                .FirstOrDefaultAsync();
            return _mapper.Map<VehicleTypeModel>(vehicleType);
        }

        public async Task<bool> UpdateActiveStatusAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.Where(x => x.VehicleId == vehicleId)
                    .FirstOrDefaultAsync();

            if(vehicle is not null)
            {
                // update status
                vehicle.IsActive = false;

                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
