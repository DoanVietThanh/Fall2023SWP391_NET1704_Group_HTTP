using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Options;
using System.Formats.Asn1;
using System.Text.RegularExpressions;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IImageService _imageService;
        private readonly AppSettings _appSettings;

        public VehicleController(IVehicleService vehicleService,
            IOptionsMonitor<AppSettings> monitor,
            IImageService imageService)
        {
            _vehicleService = vehicleService;
            _imageService = imageService;
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
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddVehicle([FromForm] VehicleAddRequest reqObj)
        {
            // generate vehicle model
            var vehicle = reqObj.ToVehicleModel(_appSettings.DateFormat);

            // generate vehicle image
            if(reqObj.VehicleImage is not null)
            {
                var imageId = Guid.NewGuid();
                vehicle.VehicleImage = imageId.ToString();

                // upload image to cloud
                //await _imageService.UploadImageAsync(imageId, reqObj.VehicleImage);
            }

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

        [HttpGet]
        [Route("vehicles/{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var vehicle = await _vehicleService.GetAsync(id);

            if(vehicle is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy phương tiện"
                });
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = vehicle
            });
        }

        [HttpGet]
        [Route("vehicles")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllVehicle()
        {
            var vehicles = await _vehicleService.GetAllAsync();

            if(vehicles.Count() > 0)
            {
                return Ok(new BaseResponse { 
                    StatusCode = StatusCodes.Status200OK,
                    Data = vehicles
                });
            }

            return NotFound(new BaseResponse { 
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Không tìm thấy phương tiện"
            });
        }

        [HttpPut]
        [Route("vehicles/{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateVehicle([FromRoute] int id,
            [FromForm] VehicleUpdateRequest reqObj)
        {
            // get by id
            var vehicle = await _vehicleService.GetAsync(id);
            if (vehicle is null)
            {
                return NotFound(new BaseResponse {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy phương tiện"
                });
            }

            // generate vehicle model
            var vehicleModel = reqObj.ToVehicleModel(_appSettings.DateFormat);

            // update image (if any)
            var imageId = Guid.NewGuid().ToString();
            vehicleModel.VehicleImage = imageId;

            // remove prev image
            await _imageService.DeleteImageAsync(
                    Guid.Parse(vehicle.VehicleImage));
            // update load new image
            await _imageService.UploadImageAsync(Guid.Parse(imageId),
                reqObj.Image);

            // update vehicle
            bool isSucess = await _vehicleService.UpdateAsync(id, vehicleModel);
            if (isSucess)
            {
                return Ok(new BaseResponse {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Sửa phương tiện thành công"
                });
            }

            return new ObjectResult(new BaseResponse { 
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Sửa phương tiện thất bại"
            }) 
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        [HttpDelete]
        [Route("vehicles/{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] int id) 
        {
            // get by id 
            var vehicle = await _vehicleService.GetAsync(id);
            if (vehicle is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy phương tiện"
                });
            }

            // delete vehicle
            // update vehicle
            bool isSucess = await _vehicleService.DeleteAsync(id);
            if (isSucess)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Xóa phương tiện thành công"
                });
            }

            return new ObjectResult(new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Xóa phương tiện thất bại"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
