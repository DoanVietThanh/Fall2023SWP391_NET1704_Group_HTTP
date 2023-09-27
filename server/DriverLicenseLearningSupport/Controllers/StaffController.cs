﻿using DocumentFormat.OpenXml.Spreadsheet;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IJobTitleService _jobTitleService;
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;
        private readonly IStaffService _staffService;
        private readonly IImageService _imageService;
        private readonly IAddressService _addressService;
        private readonly IFeedbackService _feedBackService;
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;

        public StaffController(ILicenseTypeService licenseTypeService,
            IJobTitleService jobTitleService,
            IRoleService roleService,
            IAccountService accountService,
            IStaffService staffService,
            IImageService imageService,
            IAddressService addressService,
            IFeedbackService feedBackService,
            IMemoryCache cache,
            IOptionsMonitor<AppSettings> monitor)
        {
            _licenseTypeService = licenseTypeService;
            _jobTitleService = jobTitleService;
            _roleService = roleService;
            _accountService = accountService;
            _staffService = staffService;
            _imageService = imageService;
            _addressService = addressService;
            _feedBackService = feedBackService;
            _cache = cache;
            _appSettings = monitor.CurrentValue;
        }


        [HttpGet]
        [Route("staffs/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> StaffRegister()
        {
            // get all license type
            var licenseTypes = await _licenseTypeService.GetAllAsync();
            // get all job title
            var jobTitles = await _jobTitleService.GetAllAsync();
            // get all account roles
            var roles = await _roleService.GetAllAsync();

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    LicenseTypes = licenseTypes,
                    JobTitles = jobTitles,
                    Roles = roles
                }
            });
        }
        
        [HttpPost]
        [Route("staffs/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> StaffRegister([FromBody] StaffAddRequest reqObj)
        {
            // check account exist
            var accountExist = await _accountService.GetByEmailAsync(reqObj.Username);
            if (accountExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        Message = "Email already exist!"
                    });
            }

            //generate account
            var account = reqObj.ToAccountModel();
            // validate account model
            var accountValidateResult = await account.ValidateAsync();
            if (accountValidateResult is not null) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                // ValidationProblemDetails <- errors[]
                Errors = accountValidateResult
            });

            // validate address model
            // generate address
            var address = reqObj.ToAddressModel();
            // validate address model
            var addressValidateResult = await address.ValidateAsync();
            if (addressValidateResult is not null) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                // ValidationProblemDetails <- errors[]
                Errors = addressValidateResult
            });
            var addressId = Guid.NewGuid().ToString();
            address.AddressId = addressId;

            // generate staff
            var staff = reqObj.ToStaffModel(_appSettings.DateFormat);
            // validate staff model
            var staffValidateResult = await address.ValidateAsync();
            if (staffValidateResult is not null) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                // ValidationProblemDetails <- errors[]
                Errors = staffValidateResult
            });
            var staffId = Guid.NewGuid().ToString();
            staff.StaffId = staffId;
            staff.EmailNavigation = account;
            staff.Address = address;
            staff.IsActive = true;
            staff.AvatarImage = _appSettings.DefaultAvatar;

            // create staff
            await _staffService.CreateAsync(staff);

            // find created staff by id
            staff = await _staffService.GetAsync(Guid.Parse(staffId));

            // clear cache 
            if (_cache.Get(_appSettings.StaffsCacheKey) is not null)
            {
                _cache.Remove(_appSettings.StaffsCacheKey);
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Register Sucess",
                Data = new
                {
                    Staff = staff
                }
            });
        }

        [HttpGet]
        [Route("staffs/{id:Guid}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetStaff([FromRoute] Guid id) 
        {
            // get staff by id
            var staff = await _staffService.GetAsync(id);
            // not found
            if (staff is null) 
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound, 
                    Message = $"Not found any staff match id {id}"
                });
            }

            // 200 OK <- found
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = staff
            });
        }

        [HttpGet]
        [Route("staffs/{page:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllStaff([FromRoute] int page = 1) 
        {
            // memory cache
            if (!_cache.TryGetValue(_appSettings.StaffsCacheKey, out IEnumerable<StaffModel> staffs))
            {
                //get all staff
                staffs = await _staffService.GetAllAsync();

                // set memory cache
                var cacheOptions = new MemoryCacheEntryOptions()
                    // none access exceeds 45s <- cache expired
                    .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                    // after 10m from first access <- cache expired
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(600))
                    // cache priority
                    .SetPriority(CacheItemPriority.Normal);
                // set cache
                var test = _cache.Set(_appSettings.StaffsCacheKey, staffs, cacheOptions);
            }
            else
            {
                // get from memory cache
                staffs = (IEnumerable<StaffModel>)_cache.Get(_appSettings.StaffsCacheKey);
            }

            // 404 Not found <- not found any staff
            if (staffs is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any staff"
            });

            // paging
            var result = PaginatedList<StaffModel>.CreateByIEnumerable(staffs, page, _appSettings.PageSize);

            // 200 Ok <- found
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = new {
                    Staffs = result,
                    PageIndex = result.PageIndex,
                    TotalPage = result.TotalPage
                }
            });
        }

        [HttpGet]
        [Route("staffs/{page:int}/filters")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllStaffFilter([FromQuery] StaffFilter filters, [FromRoute] int page = 1)
        {
            // get staffs by filtering
            var staffs = await _staffService.GetAllByFilterAsync(filters);

            // 404 Not Found <- not found any staff match filters
            if (staffs is null) return NotFound(new BaseResponse { 
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any staffs"
            });

            // paging
            var result = PaginatedList<StaffModel>.CreateByIEnumerable(staffs, page, _appSettings.PageSize);

            // 200 OK <- found
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new { 
                    Staffs = result,
                    PageIndex = result.PageIndex,
                    TotalPage = result.TotalPage
                }
            });
        }

        [HttpGet]
        [Route("staffs/mentors/{page:int}")]
        public async Task<IActionResult> GetAllMentor([FromRoute] int page = 1)
        {
            // get all mentors
            var mentors = await _staffService.GetAllMentorAsync();

            // get all mentors feeback
            foreach(var mentor in mentors) 
            {
                var feedbacks = await _feedBackService.GetAllMentorFeedback(Guid.Parse(mentor.StaffId));
                mentor.FeedBacks = feedbacks.ToList();
            }

            // check exist
            if(mentors is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Not found any mentors"
                });
            }

            // paging
            var result = PaginatedList<StaffModel>.CreateByIEnumerable(mentors, page, _appSettings.PageSize);

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = new {
                    Mentors = result,
                    PageIndex = result.PageIndex,
                    TotalPage = result.TotalPage
                }
            });

        }

        [HttpPut]
        [Route("staffs/{id:Guid}/update")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStaff([FromRoute] Guid id, [FromForm] StaffUpdateRequest reqObj) 
        {
            // get staff by id
            var staff = await _staffService.GetAsync(id);

            // 404 Not Found <- not found staff match id
            if (staff is null) 
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any staff match id {id}"
                });
            }

            // generate address model
            var address = reqObj.ToAddressModel();

            // generate staff model
            var staffModel = reqObj.ToStaffModel(_appSettings.DateFormat);
            staffModel.StaffId = id.ToString();
            staffModel.Address = address;
            staffModel.IsActive = true;

            // update avatar image
            // 1. generate image id
            var imageId = Guid.NewGuid();
            // 2. remove prev image
            await _imageService.DeleteImageAsync(Guid.Parse(staff.AvatarImage));
            // 3. upload new image
            await _imageService.UploadImageAsync(imageId, reqObj.AvatarImage);
            // 4. save db
            staffModel.AvatarImage = imageId.ToString();

            // update staff
            var isSucess = await _staffService.UpdateAsync(id, staffModel);

            // update success
            if (isSucess)
            {
                // 200 Ok <- update sucessfully
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Update staff id {id} succesfully"
                });
            }

            // 500 Internal <- cause error
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete]
        [Route("staffs/{id:Guid}/delete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteStaff([FromRoute] Guid id) 
        {
            // get staff
            var staff = await _staffService.GetAsync(id);
            // not exist
            if (staff is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any members match id {id}"
            });
            // delete staff
            await _staffService.DeleteAsync(id);
            // delete account
            await _accountService.DeleteAsync(staff.Email);
            // delete address
            await _addressService.DeleteAsync(Guid.Parse(staff.AddressId));

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = $"Delete member {id} successfully"
            });
        }

        // import excel
        [HttpPost]
        [Route("staffs/import-excel")]
        public async Task<IActionResult> ImportToExcel([FromForm] IFormFile file, [Required] string licenseType,
            [Required] int jobTitleId, [Required] int roleId)
        {
            
            // validate excel file
            var validator = new ExcelFileValidator();
            var result = await validator.ValidateAsync(file);
            if (result is not null) // cause error
            {
                // return ValidationProblemDetails <- error[]
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = result.ToProblemDetails()
                });
            }

            if (file?.Length > 0) 
            {
                // convert file to stream
                var stream = file.OpenReadStream();

                // pass file stream to excel package
                using (var xlPackage = new ExcelPackage(stream))
                {
                    // get first worksheet of package
                    var worksheet = xlPackage.Workbook.Worksheets.First();
                    // count row
                    var rowCount = worksheet.Dimension.Rows;

                    // generate model <- each row
                    // default first row data is 2
                    for (var row = 2; row <= rowCount; ++row)
                    {
                        var email = worksheet.Cells[row, 1].Value.ToString();
                        var password = PasswordHelper.ConvertToEncrypt(worksheet.Cells[row, 2].Value.ToString());
                        var firstName = worksheet.Cells[row, 3].Value.ToString();
                        var lastName = worksheet.Cells[row, 4].Value.ToString();
                        var dateBirth = DateTime.ParseExact(worksheet.Cells[row, 5].Value.ToString(),
                            _appSettings.DateFormat, CultureInfo.InvariantCulture);
                        var phone = worksheet.Cells[row, 6].Value.ToString();
                        var street = worksheet.Cells[row, 7].Value.ToString();
                        var district = worksheet.Cells[row, 8].Value.ToString();
                        var city = worksheet.Cells[row, 9].Value.ToString();
                        var linceseType = worksheet.Cells[row, 10].Value.ToString();

                        // get license type by description
                        var licenseTypeId = await _licenseTypeService.GetByDescAsync(linceseType.ToUpper());

                        // generate account model
                        var account = new AccountModel
                        {
                            Email = email,
                            Password = PasswordHelper.ConvertToEncrypt(password),
                            IsActive = true,
                            RoleId = roleId
                        };
                        // account validation
                        var accountValidateResult = await account.ValidateAsync();
                        if (accountValidateResult is not null)
                        {
                            return BadRequest(new ErrorResponse
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = $"Cause error at staff {firstName} {lastName}, row {row}",
                                Errors = accountValidateResult
                            });
                        }

                        // generate address model
                        var address = new AddressModel
                        {
                            AddressId = Guid.NewGuid().ToString(),
                            City = city,
                            District = district,
                            Street = street
                        };
                        // address validation
                        var addressValidateResult = await address.ValidateAsync();
                        if (addressValidateResult is not null)
                        {
                            return BadRequest(new ErrorResponse
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = $"Cause error at staff {firstName} {lastName}, row {row}",
                                Errors = addressValidateResult
                            });
                        }

                        // generate staff model
                        var staff = new StaffModel
                        {
                            StaffId = Guid.NewGuid().ToString(),
                            FirstName = firstName,
                            LastName = lastName,
                            DateBirth = dateBirth,
                            Phone = phone,
                            AvatarImage = _appSettings.DefaultAvatar,
                            EmailNavigation = account,
                            Address = address,
                            LicenseTypeId = Convert.ToInt32(licenseTypeId),
                            JobTitleId = jobTitleId
                        };
                        // staff validation
                        var staffValidateResult = await staff.ValidateAsync();
                        if (staffValidateResult is not null)
                        {
                            return BadRequest(new ErrorResponse
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = $"Cause error at staff {firstName} {lastName}, row {row}",
                                Errors = staffValidateResult
                            });
                        }

                        // create staff <- no cause errors
                        await _staffService.CreateAsync(staff);
                    }

                    // Get total staffs
                    var totalMembers = rowCount - 1;

                    // clear cache
                    if (_cache.Get(_appSettings.StaffsCacheKey) is not null)
                        _cache.Remove(_appSettings.StaffsCacheKey);

                    // Import Sucessfully
                    if (totalMembers > 0) return Ok(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = $"Import excel file successfully, total {totalMembers} members created"
                    });
                }
            }
            // Import Failed
            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Import excel file failed."
            });
        }

        // export excel
        [HttpGet]
        [Route("staffs/export-excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] StaffFilter filters) 
        {
            var staffs = await _staffService.GetAllByFilterAsync(filters);
            if(staffs is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Not found any staffs to export excel"
                });
            }

            // start exporting to excel
            // create file stream
            var stream = new MemoryStream();

            // create excel package
            using (var xlPackage = new ExcelPackage(stream)) 
            {
                // define a worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Staffs");
                // first row
                var startRow = 3;
                // worksheet details
                worksheet.Cells["A1"].Value = "List of Staffs";
                using(var r = worksheet.Cells["A1:C1"])
                {
                    r.Merge = true;
                }
                // table header
                worksheet.Cells["A2"].Value = "Id";
                worksheet.Cells["B2"].Value = "First Name";
                worksheet.Cells["C2"].Value = "Last Name";
                worksheet.Cells["D2"].Value = "Date Birth";
                worksheet.Cells["E2"].Value = "Phone";
                worksheet.Cells["F2"].Value = "Email";
                worksheet.Cells["G2"].Value = "Street";
                worksheet.Cells["H2"].Value = "District";
                worksheet.Cells["I2"].Value = "City";
                worksheet.Cells["J2"].Value = "License Type";
                worksheet.Cells["K2"].Value = "Job Title";

                // table row
                var row = startRow;
                foreach (var s in staffs)
                {
                    // set row record
                    worksheet.Cells[row, 1].Value = s.StaffId.ToString();
                    worksheet.Cells[row, 2].Value = s.FirstName;
                    worksheet.Cells[row, 3].Value = s.LastName;
                    worksheet.Cells[row, 4].Value = s.DateBirth.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 5].Value = s.Phone;
                    worksheet.Cells[row, 6].Value = s.Email;
                    worksheet.Cells[row, 7].Value = s.Address.Street;
                    worksheet.Cells[row, 8].Value = s.Address.District;
                    worksheet.Cells[row, 9].Value = s.Address.City;
                    worksheet.Cells[row, 10].Value = s.LicenseType.LicenseTypeDesc;
                    worksheet.Cells[row, 11].Value = s.JobTitle.JobTitleDesc;
                    
                    // next row
                    ++row;

                }
                // properties
                xlPackage.Workbook.Properties.Title = "Staff List";
                xlPackage.Workbook.Properties.Author = "Admin  ";
                await xlPackage.SaveAsync();

            }
            // read from position
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "staffs.xlsx");
        }
    }
}