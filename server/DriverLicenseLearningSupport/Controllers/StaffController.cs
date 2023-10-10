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
        private readonly IWeekDayScheduleService _weekDayScheduleService;
        private readonly ICourseService _courseService;
        private readonly ISlotService _slotService;
        private readonly ITeachingScheduleService _teachingScheduleService;

        //private readonly IVehicleService _vehicleService;
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
            IWeekDayScheduleService weekDayScheduleService,
            ICourseService courseService,
            ISlotService slotService,
            ITeachingScheduleService teachingScheduleService,
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
            _weekDayScheduleService = weekDayScheduleService;
            _courseService = courseService;
            _slotService = slotService;
            _teachingScheduleService = teachingScheduleService;
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
        [Authorize(Roles = "Admin,Staff")]
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

        [HttpGet]
        [Route("staffs/{id:Guid}/schedule")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetMentorSchedule([FromRoute] Guid id)
        {
            // generate current date time
            var currDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), _appSettings.DateFormat, CultureInfo.InvariantCulture);
            // get calendar by current date
            var weekday = await _weekDayScheduleService.GetByDateAsync(currDate);
            // get all weekday of calendar
            var weekdays = await _weekDayScheduleService.GetAllAsync();
            // get all slots 
            var slots = await _slotService.GetAllAsync();
            // convert to list of course
            var listOfSlotSchedule = slots.ToList();
            // get all teaching schedule of mentor

            // get teaching schedule for each slot
            foreach(var s in slots)
            {
                var teachingSchedules
                    = await _teachingScheduleService.GetBySlotAndWeekDayScheduleAsync(s.SlotId, 
                        weekday.WeekdayScheduleId, id);
                s.TeachingSchedules = teachingSchedules.ToList();
            }
            // get course by id 
            var course = await _courseService.GetAsync(Guid.Parse(weekday.CourseId));
            
            // response
            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Course = course,
                    Filter = weekdays.Select(x => new {
                        Id = x.WeekdayScheduleId,
                        Desc = x.WeekdayScheduleDesc
                    }),
                    Weekdays = weekday,
                    SlotSchedules = listOfSlotSchedule
                }
            });
        }

        [HttpGet]
        [Route("staffs/{id:Guid}/schedule/filter")]
        public async Task<IActionResult> GetMentorScheduleByFilter([FromRoute] Guid id,
            [FromQuery] TeachingScheduleFilter filters)  
        {
            // get teaching date by filters
            var teachingDate = await _teachingScheduleService.GetByFilterAsync(filters);

            if (teachingDate is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any teaching schedule match required"
                });
            }


            // get calendar by id
            var weekday = await _weekDayScheduleService.GetAsync(
                Convert.ToInt32(teachingDate.WeekdayScheduleId));
            // get all weekday of calendar
            var weekdays = await _weekDayScheduleService.GetAllAsync();
            // get all slots 
            var slots = await _slotService.GetAllAsync();
            // convert to list of course
            var listOfSlotSchedule = slots.ToList();
            // get teaching schedule for each slot
            foreach (var s in slots)
            {
                var teachingSchedules
                    = await _teachingScheduleService.GetBySlotAndWeekDayScheduleAsync(s.SlotId,
                        weekday.WeekdayScheduleId, id);
                s.TeachingSchedules = teachingSchedules.ToList();
            }
            // get course by id 
            var course = await _courseService.GetAsync(Guid.Parse(weekday.CourseId));
            course.Mentors = null;

            // response
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Course = course,
                    Filter = weekdays.Select(x => new {
                        Id = x.WeekdayScheduleId,
                        Desc = x.WeekdayScheduleDesc
                    }),
                    Weekdays = weekday,
                    SlotSchedules = listOfSlotSchedule
                }
            });
        }

        [HttpGet]
        [Route("staffs/mentors/{id:Guid}/schedule-register")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> TeachingScheduleRegister([FromRoute] Guid id) 
        {
            // get course by mentor id
            var course = await _courseService.GetByMentorIdAsync(id);
            // not found
            if(course is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Mentor {id} have not taught any course yet"
                });
            }

            // get slots
            var slots = await _slotService.GetAllAsync();

            // 404 Not found 
            if(slots is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Not found any slots"
                });
            }

            // 200Ok <- found
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = slots
            });
        }

        [HttpPost]
        [Route("staffs/mentors/schedule-register")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> TeachingScheduleRegister([FromBody] TeachingScheduleRequest reqObj)
        {
            // get course by mentor id
            var course = await _courseService.GetByMentorIdAsync(Guid.Parse(reqObj.MentorId));
            // check teaching exist
            if(course is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any course of mentor id {reqObj.MentorId}"
                });
            }

            // get weekday schedule id by teaching date request
            var weekday = await _weekDayScheduleService.GetByDateAsync(reqObj.TeachingDate);
            // check teaching date exist
            if (weekday is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any date match {reqObj.TeachingDate.ToString("dd/MM/yyyy")} " +
                    $"in schedule of course {course.CourseTitle}"
                });
            }

            // generate teaching schedule model
            var teachingSchedule = reqObj.ToScheduleModel();
            teachingSchedule.WeekdayScheduleId = weekday.WeekdayScheduleId;

            // check already exist teaching date with particular slot
            var existSchedule = await _teachingScheduleService.GetByMentorIdAndTeachingDateAsync(
                    Guid.Parse(reqObj.MentorId), reqObj.TeachingDate, reqObj.SlotId);

            if (existSchedule is null)
            {
                var createdSchedule = await _teachingScheduleService.CreateAsync(teachingSchedule);
                if (createdSchedule is not null)
                {
                    return new ObjectResult(createdSchedule) { StatusCode = StatusCodes.Status200OK };
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest(new BaseResponse { 
                StatusCode = StatusCodes.Status400BadRequest,
                Message = $"Ngày đăng ký và slot đăng ký đã có trong lịch dạy, vui lòng chọn lại"
            });
        }

        [HttpGet]
        [Route("staffs/update")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStaff() 
        {
            // get all job title
            var jobTitles = await _jobTitleService.GetAllAsync();
            // get all license types 
            var licenseTypes = await _licenseTypeService.GetAllAsync();
            // response
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new { LicenseTypes = licenseTypes, JobTitles = jobTitles }
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
            //await _imageService.DeleteImageAsync(Guid.Parse(staff.AvatarImage));
            // 3. upload new image
            //await _imageService.UploadImageAsync(imageId, reqObj.AvatarImage);
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
        public async Task<IActionResult> ImportToExcel(IFormFile file,
            int jobTitleId, int roleId)
        {
            
            // validate excel file
            var validator = new ExcelFileValidator();
            var result = await validator.ValidateAsync(file);
            if (!result.IsValid) // cause error
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
                            "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                        var phone = "0" + worksheet.Cells[row, 6].Value.ToString();
                        var street = worksheet.Cells[row, 7].Value.ToString();
                        var district = worksheet.Cells[row, 8].Value.ToString();
                        var city = worksheet.Cells[row, 9].Value.ToString();
                        var linceseType = worksheet.Cells[row, 10].Value.ToString();

                        // check exist email
                        var existEmail = await _accountService.GetByEmailAsync(email);
                        if (existEmail is not null) return BadRequest(new BaseResponse { 
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = $"Email of {firstName} {lastName}, row {row} already exist",
                        });

                        // get license type by description
                        var licenseTypeModel = await _licenseTypeService.GetByDescAsync(linceseType.ToUpper());

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
                            LicenseTypeId = Convert.ToInt32(licenseTypeModel.LicenseTypeId),
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
                    var totalStaffs = rowCount - 1;

                    // clear cache
                    if (_cache.Get(_appSettings.StaffsCacheKey) is not null)
                        _cache.Remove(_appSettings.StaffsCacheKey);

                    // Import Sucessfully
                    if (totalStaffs > 0) return Ok(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = $"Import excel file successfully, total {totalStaffs} staff created"
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
