using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepo;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepo,
            IMapper mapper)
        {
            _vehicleRepo = vehicleRepo;
            _mapper = mapper;
        }

        public async Task<VehicleModel> CreateAsync(VehicleModel model)
        {
            var vehicleEntity = _mapper.Map<Vehicle>(model);
            return await _vehicleRepo.CreateAsync(vehicleEntity);
        }

        public async Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAynsc()
        {
            return await _vehicleRepo.GetAllVehicleTypeAsync();
        }
    }
}
