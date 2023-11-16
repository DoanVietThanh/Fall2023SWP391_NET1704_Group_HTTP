using Amazon.Runtime.Internal.Auth;
using DocumentFormat.OpenXml.Drawing;
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
using OfficeOpenXml;
using System.Formats.Asn1;
using System.Globalization;
using System.IO.Packaging;
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
                await _imageService.UploadImageAsync(imageId, reqObj.VehicleImage);
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

            if (vehicles.Count() == 0)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy phương tiện"
                });
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = vehicles
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

            if(reqObj.Image is not null)
            {
                // update image (if any)
                var imageId = Guid.NewGuid().ToString();
                vehicleModel.VehicleImage = imageId;

                // remove prev image
                await _imageService.DeleteImageAsync(
                        Guid.Parse(vehicle.VehicleImage));
                // update load new image
                await _imageService.UploadImageAsync(Guid.Parse(imageId),
                    reqObj.Image);
            }

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

        [HttpGet]
        [Route("vehicles/export-excel")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ExportToExcel()
        {
            // get all vehicles
            var vehicles = await _vehicleService.GetAllAsync();
            if(vehicles.Count() == 0)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Hiện chưa có phương tiện"
                });
            }

            // export to excel
            // create file stream
            var stream = new MemoryStream();

            using(var xlPackage = new OfficeOpenXml.ExcelPackage(stream))
            {
                // define a worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Vehicles");

                // first row reader
                var startRow = 3;

                // worksheet details
                worksheet.Cells["A1"].Value = "Danh sách xe chưa được bàn giao";
                using(var r = worksheet.Cells["A1:C1"])
                {
                    r.Merge = true;
                }

                // table header
                worksheet.Cells["A2"].Value = "ID";
                worksheet.Cells["B2"].Value = "Tên xe";
                worksheet.Cells["C2"].Value = "Biển số";
                worksheet.Cells["D2"].Value = "Ngày đăng ký xe";
                worksheet.Cells["E2"].Value = "Loại xe";
                worksheet.Cells["F2"].Value = "Trạng thái";


                var row = startRow;
                foreach(var v in vehicles)
                {
                    // set row record
                    worksheet.Cells[row, 1].Value = v.VehicleId.ToString();
                    worksheet.Cells[row, 2].Value = v.VehicleName.ToString();
                    worksheet.Cells[row, 3].Value = v.VehicleLicensePlate.ToString();
                    worksheet.Cells[row, 4].Value = Convert.ToDateTime(v.RegisterDate).ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 5].Value = v.VehicleType.VehicleTypeDesc.ToString();
                    worksheet.Cells[row, 6].Value = (v.IsActive) ? "Có sẵn" : "Đã được bàn giao";

                    ++row;
                }

                // properties
                xlPackage.Workbook.Properties.Title = "Danh sách xe";
                xlPackage.Workbook.Properties.Author = "Admin";
                await xlPackage.SaveAsync();
            }

            // read from position
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Danh sách xe.xlsx");
        }

        [HttpPost]
        [Route("vehicles/import-excel")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            // validate excel file
            var validator = new ExcelFileValidator();
            var result = await validator.ValidateAsync(file);
            // !isValid <- cause error
            if (!result.IsValid)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "File không hợp lệ, vui lòng chọn file excel"
                });
            }

            // covert to stream
            var stream = file.OpenReadStream();

            // excel package 
            using(var xlPackage = new OfficeOpenXml.ExcelPackage(stream))
            {
                // define worksheet
                var worksheet = xlPackage.Workbook.Worksheets.First();
                // count row 
                var rowCount = worksheet.Dimension.Rows;
                // first row <- skip header
                var firstRow = 2;

                // generate model <- each row
                for(int i = firstRow; i <= rowCount; ++i)
                {
                    var vehicleName = worksheet.Cells[i, 1].Value.ToString();
                    var licensePlate = worksheet.Cells[i, 2].Value.ToString();
                    var registerDate = worksheet.Cells[i, 3].Value.ToString();
                    var vehicleType = worksheet.Cells[i,4].Value.ToString();
                    var status = worksheet.Cells[i, 5].Value.ToString();


                    // validation
                    if(!DateTime.TryParse(registerDate, out var formatDate))
                    {
                        return BadRequest(new BaseResponse
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = $"Ngày đăng ký xe tại hàng {i} không hợp lệ, '{formatDate}'"
                        });
                    }

                    // get vehicle type by id
                    var vehicleTypeModel = await _vehicleService.GetVehicleTypeByDescAsync(vehicleType);

                    // generate vehicle model
                    var vehicleModel = new VehicleModel 
                    {
                        VehicleName = vehicleName,
                        VehicleLicensePlate = licensePlate,
                        RegisterDate = DateTime.ParseExact(Convert.ToDateTime(registerDate).ToString(_appSettings.DateFormat),
                            _appSettings.DateFormat,
                            CultureInfo.InvariantCulture),
                        VehicleTypeId = vehicleTypeModel.VehicleTypeId,
                        IsActive = (status.Equals("Có sẵn")) ? true : false
                    };

                    // create vehicle
                    var isCreatedModel = await _vehicleService.CreateAsync(vehicleModel);

                    if(isCreatedModel is null)
                    {
                        return BadRequest(new BaseResponse { 
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = $"Xảy ra lỗi tại hàng {i}"
                        });
                    }
                }

                var totalAdded = rowCount - 1;

                if(totalAdded > 0)
                {
                    return Ok(new BaseResponse { 
                        StatusCode = StatusCodes.Status200OK,
                        Message = $"Import excel thành công, tổng {totalAdded} xe được thêm mới"
                    });
                }

                return new ObjectResult(new BaseResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Import excel thất bại"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

        }
    }
}
