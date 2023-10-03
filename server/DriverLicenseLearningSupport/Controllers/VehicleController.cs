using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly AppSettings _appSettings;

        public VehicleController(IVehicleService vehicleService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _vehicleService = vehicleService;
            _appSettings = monitor.CurrentValue;
        }

        [HttpGet]
        [Route("vehicles/types")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetVehicleTypes()
        {
            var vehicleTypes = await _vehicleService.GetAllVehicleTypeAynsc();

            if(vehicleTypes is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Not found any vehicle types"
                });
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = vehicleTypes
            });
        }

        [HttpPost]
        [Route("vehicles/add")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleAddRequest reqObj)
        {
            // generate vehicle model
            var vehicle = reqObj.ToVehicleModel(_appSettings.DateFormat);
            // validate
            var vehicleValidateResult = await vehicle.ValidateAsync();
            if(vehicleValidateResult is not null)
            {
                return BadRequest(new ErrorResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = vehicleValidateResult
                });
            }

            // create vehicle
            var createdVehicle = await _vehicleService.CreateAsync(vehicle);

            // 201 Created <- create success
            if(createdVehicle is not null) 
            {
                return new ObjectResult(createdVehicle) { StatusCode = StatusCodes.Status200OK };
            }

            // 500 Internal <- cause error
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
