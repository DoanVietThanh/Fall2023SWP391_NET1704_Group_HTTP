﻿using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;
using System.Runtime.InteropServices;

namespace DriverLicenseLearningSupport.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepo;
        private readonly ITeachingScheduleService _teachingScheduleService;
        private readonly IWeekDayScheduleService _weekdayService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepo,
            ITeachingScheduleService teachingScheduleService,
            IWeekDayScheduleService weekdayService,
            IImageService imageService,
            IMapper mapper)
        {
            _vehicleRepo = vehicleRepo;
            _teachingScheduleService = teachingScheduleService;
            _weekdayService = weekdayService;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<VehicleModel> CreateAsync(VehicleModel model)
        {
            var vehicleEntity = _mapper.Map<Vehicle>(model);
            return await _vehicleRepo.CreateAsync(vehicleEntity);
        }

        public async Task<VehicleModel> GetVehicleInDateScheduleAsync(DateTime teachingDate, int vehicleTypeId)
        {
            var vehicles = await _vehicleRepo.GetAllByVehicleTypeIdAsync(vehicleTypeId);
            var schedules = await _teachingScheduleService.GetAllByTeachingDateAsync(teachingDate);

            var vehicleIds = schedules.Select(x => Convert.ToInt32(x.VehicleId)).ToList();

            if (schedules.Count() == 0) return vehicles?.FirstOrDefault();

            foreach(var vehicle in vehicles)
            {
                if (!vehicleIds.Contains(vehicle.VehicleId))
                {
                    return vehicle;
                }
            }

            //foreach (var schedule in schedules)
            //{
            //    foreach(var vehicle in vehicles) 
            //    {
            //        //var weekdaySchedule 
            //        //    = await _weekdayService.GetAsync(
            //        //        Convert.ToInt32(schedule.WeekdayScheduleId));

            //        //if (schedule.VehicleId != vehicle.VehicleId 
            //        //    && weekdaySchedule.CourseId == schedule.)
            //            return vehicle;
            //    }
            //}
            return null!;
        }

        public async Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAynsc()
        {
            return await _vehicleRepo.GetAllVehicleTypeAsync();
        }

        public async Task<VehicleTypeModel> GetVehicleTypeByLicenseTypeAsync(int licenseTypeId)
        {
            return await _vehicleRepo.GetVehicleTypeByLicenseTypeAsync(licenseTypeId);
        }

        public async Task<VehicleModel> GetVehicleByVehicleTypeAsync(int vehicleTypeId)
        {
            return await _vehicleRepo.GetVehicleByVehicleTypeAsync(vehicleTypeId);
        }

        public async Task<bool> UpdateActiveStatusAsync(int vehicleId)
        {
            return await _vehicleRepo.UpdateActiveStatusAsync(vehicleId);
        }

        public async Task<IEnumerable<VehicleModel>> GetAllActiveVehicleByType(int vehicleTypeId)
        {
            var vehicles = await _vehicleRepo.GetAllActiveVehicleByType(vehicleTypeId);
            foreach(var v in vehicles)
            {
                if(v.VehicleImage is not null)
                {
                    v.VehicleImage = await _imageService.GetPreSignedURL(Guid.Parse(v.VehicleImage));
                }
            }
            return vehicles;
        }

        public async Task<IEnumerable<VehicleModel>> GetAllInActiveVehicleByType(int vehicleTypeId)
        {
            return await _vehicleRepo.GetAllInActiveVehicleByType(vehicleTypeId);
        }

        public async Task<VehicleModel> GetAsync(int vehicleId)
        {
            return await _vehicleRepo.GetAsync(vehicleId);
        }

        public async Task<bool> UpdateAsync(int vehicleId, VehicleModel vehicle)
        {
            return await _vehicleRepo.UpdateAsync(vehicleId, vehicle);
        }

        public async Task<bool> DeleteAsync(int vehicleId)
        {
            return await _vehicleRepo.DeleteAsync(vehicleId);
        }

        public async Task<IEnumerable<VehicleModel>> GetAllAsync()
        {
            return await _vehicleRepo.GetAllAsync();
        }

        public async Task<VehicleTypeModel> GetVehicleTypeByDescAsync(string vehicleTypeDesc)
        {
            return await _vehicleRepo.GetVehicleTypeByDescAsync(vehicleTypeDesc);
        }
    }
}
