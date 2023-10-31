using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

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

        public async Task<IEnumerable<VehicleModel>> GetAllActiveVehicleByType(int vehicleTypeId)
        {
            var vehicles = await _context.Vehicles.Where(x => x.IsActive == true
                                                           && x.VehicleTypeId == vehicleTypeId)
                                                  .ToListAsync();
            return _mapper.Map<IEnumerable<VehicleModel>>(vehicles);
        }
        public async Task<IEnumerable<VehicleModel>> GetAllInActiveVehicleByType(int vehicleTypeId)
        {
            var vehicles = await _context.Vehicles.Where(x => x.IsActive == false
                                                           && x.VehicleTypeId == vehicleTypeId)
                                                  .ToListAsync();
            return _mapper.Map<IEnumerable<VehicleModel>>(vehicles);
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

        public async Task<VehicleModel> GetAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.Where(x => x.VehicleId == vehicleId)
                                                 .FirstOrDefaultAsync();
            return _mapper.Map<VehicleModel>(vehicle);
        }

        public async Task<bool> UpdateAsync(int vehicleId, VehicleModel vehicle)
        {
            var vehicleEntity = await _context.Vehicles.Where(x => x.VehicleId == vehicleId)
                                                       .FirstOrDefaultAsync();

            if (vehicleEntity is null) return false;

            // update vehicle
            vehicleEntity.VehicleLicensePlate = vehicle.VehicleLicensePlate;
            vehicleEntity.VehicleImage = vehicle.VehicleImage;
            vehicleEntity.VehicleName = vehicle.VehicleName;
            vehicleEntity.RegisterDate = vehicle.RegisterDate;
            vehicleEntity.VehicleTypeId = vehicle.VehicleTypeId;

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.Where(x => x.VehicleId == vehicleId)
                    .FirstOrDefaultAsync();

            if (vehicle is null) return false;

            _context.Vehicles.Remove(vehicle);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<VehicleModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<VehicleModel>>(await _context.Vehicles.ToListAsync());
        }
    }
}
