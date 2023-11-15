using Amazon.S3.Model;
using Amazon.S3.Model.Internal.MarshallTransformations;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
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
using RestSharp;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
        private readonly IRollCallBookService _rcbService;
        private readonly ICoursePackageReservationService _packageService;
        private readonly IVehicleService _vehicleService;
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;
        private readonly CourseSettings _courseSettings;

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
            IRollCallBookService rcbService,
            ICoursePackageReservationService packageService,
            IVehicleService vehicleService,
            IMemoryCache cache,
            IOptionsMonitor<AppSettings> monitor,
            IOptionsMonitor<CourseSettings> courseMonitor)
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
            _rcbService = rcbService;
            _packageService = packageService;
            _vehicleService = vehicleService;
            _cache = cache;
            _appSettings = monitor.CurrentValue;
            _courseSettings = courseMonitor.CurrentValue;
        }


        /// <summary>
        /// Get all neccessary features to create new staff
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> StaffRegister()
        {
            //// get all license type
            //var licenseTypes = await _licenseTypeService.GetAllAsync();
            // get all job title
            var jobTitles = await _jobTitleService.GetAllAsync();
            // get all account roles
            var roles = await _roleService.GetAllAsync();

            // get all courses
            var courses = await _courseService.GetAllAsync();

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    //LicenseTypes = licenseTypes,
                    JobTitles = jobTitles,
                    Roles = roles,
                    Courses = courses
                }
            });
        }

        /// <summary>
        /// Create new staff with different job-title (Admin,Staff,Mentor)
        /// </summary>
        /// <param name="reqObj"></param>
        /// <returns></returns>
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
                        Message = "Email đã tồn tại!"
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

            // validate birthdate
            var currDate = DateTime.ParseExact(DateTime.Now.ToString(_appSettings.DateFormat),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);
            var birthdate = staff.DateBirth;
            if (birthdate >= currDate)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ngày sinh không hợp lệ"
                });
            }

            // is mentor 
            if (staff.JobTitle.JobTitleDesc.Equals("Mentor"))
            {
                // set empty Job Title
                staff.JobTitle = null!;
                // create mentor
                var createdMentor = await _staffService.CreateAsync(staff);


                if (createdMentor && !String.IsNullOrEmpty(reqObj.CourseId))
                {
                    var course = await _courseService.GetAsync(
                        Guid.Parse(reqObj.CourseId));
                    if(course is null)
                    {
                        return NotFound(new BaseResponse { 
                            StatusCode = StatusCodes.Status404NotFound,
                            Message = "Không tìm thấy khóa học"
                        });
                    }

                    // already taught this course
                    var courseMentor = await _courseService.GetByMentorIdAndCourseIdAsync(
                        Guid.Parse(staff.StaffId),
                        Guid.Parse(reqObj?.CourseId));
                    if (courseMentor is not null)
                    {
                        return BadRequest(new BaseResponse
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = $"Người hướng dẫn đã dạy khóa học này"
                        });
                    }

                    // Add mentor to course
                    await _courseService.AddMentorAsync(
                        Guid.Parse(course.CourseId),
                        Guid.Parse(staff.StaffId));

                    return Ok(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Tạo mới người hướng dẫn thành công",
                        Data = new
                        {
                            Staff = staff
                        }
                    });
                }else if(createdMentor && String.IsNullOrEmpty(reqObj.CourseId))
                {
                    return Ok(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Tạo mới người hướng dẫn thành công",
                        Data = new
                        {
                            Staff = staff
                        }
                    });
                }

                return new ObjectResult(new BaseResponse {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Tạo người hướng dẫn thất bại"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            // set empty Job Title
            staff.JobTitle = null!;
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
                Message = "Tạo mới thành công",
                Data = new
                {
                    Staff = staff
                }
            });
        }

        /// <summary>
        /// View staff details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/{id:Guid}")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetStaff([FromRoute] Guid id)
        {
            // get staff by id
            var staff = await _staffService.GetAsync(id);
            // not found
            if (staff is null)
            {
                return NotFound(new BaseResponse {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Không tìm thấy nhân viên"
                });
            }

            // 200 OK <- found
            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Data = staff
            });
        }

        /// <summary>
        /// View all staff
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staffs = await _staffService.GetAllAsync();

            if (staffs.Count() > 0)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = staffs
                });
            }

            return BadRequest(new BaseResponse {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Hiện chưa có nhân viên"
            });
        }

        /// <summary>
        /// View all staff with paging
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
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
                Message = "Không tìm thấy nhân viên"
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

        /// <summary>
        /// Filtering staff
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="page"></param>
        /// <returns></returns>
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
                Message = $"Không tìm thấy nhân viên"
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

        /// <summary>
        /// View all mentor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/mentors")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllMentor()
        {
            // get all mentors
            var mentors = await _staffService.GetAllMentorAsync();

            // get all mentors feeback
            foreach (var mentor in mentors)
            {
                var feedbacks = await _feedBackService.GetAllMentorFeedback(Guid.Parse(mentor.StaffId));
                mentor.FeedBacks = feedbacks.ToList();
            }

            // check exist
            if (mentors is null)
            {
                return NotFound(new BaseResponse {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy người hướng dẫn"
                });
            }

            // paging
            //var result = PaginatedList<StaffModel>.CreateByIEnumerable(mentors, page, _appSettings.PageSize);

            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Data = new {
                    Mentors = mentors,
                    //PageIndex = result.PageIndex,
                    //TotalPage = result.TotalPage
                }
            });

        }

        /// <summary>
        /// View all courses of mentor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/mentors/{id:Guid}/courses")]
        public async Task<IActionResult> GetAllTeachingCourse([FromRoute] Guid id)
        {
            var courseMentor = await _courseService.GetAllMentorCourseAsync(id);

            if (courseMentor is null)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy lịch của người hướng dẫn"
                });
            }

            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Data = courseMentor
            });
        }

        /// <summary>
        /// View mentor schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/mentors/{id:Guid}/schedule")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetMentorSchedule([FromRoute] Guid id, [FromQuery] Guid courseId)
        {
            // get mentor's teaching course
            var course = await _courseService.GetByMentorIdAndCourseIdAsync(id, courseId);
            // check teaching course exist
            if (course is null)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy lịch của người hướng dẫn"
                });
            }
            course.Mentors = null!;
            course.FeedBacks = null!;
            course.Curricula = null!;
            course.CoursePackages = null!;

            // generate current date time 
            var currDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);

            // check current date time with course start date
            var courseStartDate = DateTime.ParseExact(Convert.ToDateTime(course.StartDate).ToString("yyyy-MM-dd"),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);
            var courseTotalMonth = Convert.ToInt32(course.TotalMonth);
            if (currDate < courseStartDate &&
               currDate > courseStartDate.AddMonths(courseTotalMonth))
            {
                currDate = courseStartDate;
            }

            // get calendar by current date
            var weekday = await _weekDayScheduleService.GetByDateAndCourseId(currDate,
                Guid.Parse(course.CourseId));
            //if (weekday is null)
            //{
            //    return BadRequest(new BaseResponse
            //    {
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        Message = "Không tìm thấy lịch"
            //    });
            //}

            // get all weekday of calendar
            var weekdays = await _weekDayScheduleService.GetAllByCourseId(courseId);

            if(weekday is null)
            {
                weekday = weekdays.FirstOrDefault();
            }

            // get all slots 
            var slots = await _slotService.GetAllAsync();
            // convert to list of course
            var listOfSlotSchedule = slots.ToList();
            // get all teaching schedule of mentor

            // get teaching schedule for each slot
            foreach (var s in slots)
            {
                var teachingSchedules
                    = await _teachingScheduleService.GetBySlotAndWeekDayScheduleAsync(s.SlotId,
                        weekday.WeekdayScheduleId, id);
                s.TeachingSchedules = teachingSchedules.ToList();
            }

            // count total teaching schedule
            var totalSchedule = listOfSlotSchedule.Select(x => x.TeachingSchedules.Count)
                                                  .Count();

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
                    SlotSchedules = listOfSlotSchedule,
                    TotalSchedule = totalSchedule
                }
            });
        }

        /// <summary>
        /// Filtering mentor schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/mentors/{id:Guid}/schedule/filter")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetMentorScheduleByFilter([FromRoute] Guid id,
            [FromQuery] TeachingScheduleFilter filters)
        {

            WeekdayScheduleModel weekday = null!;
            // get weekday by id
            if (filters.WeekDayScheduleId is not null)
            {
                weekday = await _weekDayScheduleService.GetAsync(
                    Convert.ToInt32(filters.WeekDayScheduleId));
            }

            if (filters.TeachingDate is not null)
            {
                // get teaching date by filters
                var teachingDate = await _teachingScheduleService.GetByFilterAsync(filters);

                if (teachingDate is null)
                {
                    return BadRequest(new BaseResponse {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = $"Không tìm thấy lịch dạy ngày {filters.TeachingDate}"
                    });
                }

                // get calendar by id
                weekday = await _weekDayScheduleService.GetAsync(
                    Convert.ToInt32(teachingDate.WeekdayScheduleId));
            }

            // get all weekday of calendar
            var weekdays = await _weekDayScheduleService.GetAllByCourseId(
                Guid.Parse(weekday.CourseId));
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
            course.Mentors = null!;
            course.FeedBacks = null!;
            course.Curricula = null!;
            course.CoursePackages = null!;

            // count total teaching schedule
            var totalSchedule = listOfSlotSchedule.Select(x => x.TeachingSchedules.Count)
                                                  .Count();

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Course = course,
                    Filter = weekdays.Select(x => new
                    {
                        id = x.WeekdayScheduleId,
                        desc = x.WeekdayScheduleDesc
                    }),
                    Weekdays = weekday,
                    SlotSchedules = listOfSlotSchedule,
                    TotalSchedule = totalSchedule
                }
            });
        }

        /// <summary>
        /// Take attendence for member
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rcbId"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("staffs/mentors/{id:Guid}/schedule/rollcallbook/{rcbId:int}")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> RollCallBookSchedule([FromRoute] Guid id,
            [FromRoute] int rcbId,
            RollCallBookRequest reqObj) {
            
            // rollcallbook without hours, km
            if (String.IsNullOrEmpty(reqObj.TotalHoursDriven.ToString())
                && String.IsNullOrEmpty(reqObj.TotalHoursDriven.ToString()))
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Vui lòng nhập tổng quảng đường đi được và thời gian đã đi"
                });
            }

            // get mentor by id
            var mentor = await _staffService.GetAsync(id);
            if (mentor is null)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy người hướng dẫn"
                });
            }
            // get rcb by id
            var rcbook = await _rcbService.GetAsync(rcbId);
            if (rcbook is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy lịch điểm danh"
                });
            }

            // get course package by member id 
            var package = await _packageService.GetByMemberAsync(
                Guid.Parse(rcbook.MemberId));
            // get course limit hours, km
            var course = await _courseService.GetAsync(
                Guid.Parse(package.CoursePackage.CourseId));

            // generate update rcb model
            var rcbUpdateModel = reqObj.ToRollCallBookModel();

            // update rcb
            bool isSuccess = await _rcbService.UpdateAsync(rcbId, rcbUpdateModel);

            if (isSuccess)
            {
                // get rcb after update
                rcbook = await _rcbService.GetAsync(rcbId);
                // get all member rcb
                var rcbooks = await _rcbService.GetAllByMemberIdAsync(
                    Guid.Parse(rcbook.MemberId));
                // total hours driven 
                var totalHoursDriven = rcbooks.Select(x => x.TotalHoursDriven).Sum();
                // total km driven
                var totalKmDriven = rcbooks.Select(x => x.TotalKmDriven).Sum();

                var remainingHour = course.TotalHoursRequired - totalHoursDriven;
                var remainingKm = course.TotalKmRequired - totalKmDriven;

                if (remainingHour < 0 && remainingKm < 0)
                {
                    return Ok(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Điểm danh thành công, học viên đã hoàn thành " +
                    "đủ số giờ học và số km yêu cầu"
                    });
                }

                return Ok(new BaseResponse {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Điểm danh thành công",
                    Data = new
                    {
                        RemainingRequiredHour = (remainingHour > 0) ? remainingHour : 0,
                        RemainingRequiredKm = (remainingKm > 0) ? remainingKm : 0
                    }
                });

            }
            return new ObjectResult(new BaseResponse {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Điểm danh thất bại"
            });
        }

        /// <summary>
        /// Get all neccesary features to register teaching schedule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/mentors/{id:Guid}/schedule-register")]
        [Authorize(Roles = "Admin,Staff,Mentor")]
        public async Task<IActionResult> TeachingScheduleRegister([FromRoute] Guid id)
        {
            // get course by mentor id
            var courses = await _courseService.GetAllMentorCourseAsync(id);
            // not found
            if (courses is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy người hướng dẫn"
                });
            }

            // get slots
            var slots = await _slotService.GetAllAsync();

            // 404 Not found 
            if (slots is null)
            {
                return NotFound(new BaseResponse {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy slot học"
                });
            }

            // 200Ok <- found
            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    MentorCourses = courses,
                    Slots = slots
                }
            });
        }

        /// <summary>
        /// Teaching schedule register, with await status
        /// </summary>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("staffs/mentors/schedule-register")]
        [Authorize(Roles = "Admin,Staff,Mentor")]
        public async Task<IActionResult> TeachingScheduleRegister([FromBody] TeachingScheduleRequest reqObj)
        {
            if (!DateTime.TryParse(reqObj.TeachingDate.ToString(), out var date))
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Vui lòng chọn ngày đăng ký"
                });
            }

            // get mentor teaching schedule exist
            var existSchedules = await _teachingScheduleService.GetAllByMentorIdAsync(
                Guid.Parse(reqObj.MentorId));

            if (existSchedules.Count() == 0)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Vui lòng đăng ký lịch dạy cả khóa " +
                    $"trước khi đăng ký lịch dạy theo tuần"
                });
            }

            // get vehicle 
            var vehicleId = existSchedules.FirstOrDefault().VehicleId;

            // get mentor course
            var course = await _courseService.GetByMentorIdAndCourseIdAsync(Guid.Parse(reqObj.MentorId),
                Guid.Parse(reqObj.CourseId));
            if (course is null)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Người hướng dẫn không được phép đăng ký lịch"
                });
            }

            // Limit register date range
            var courseStartDate = Convert.ToDateTime(course.StartDate);
            var courseEndDate = courseStartDate.AddMonths(Convert.ToInt32(course.TotalMonth));
            if (reqObj.TeachingDate < courseStartDate ||
               reqObj.TeachingDate > courseEndDate)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không thể tạo lịch dạy, Khóa học chỉ diễn ra từ {courseStartDate.ToString("dd/MM/yyyy")}" +
                    $" đến {courseEndDate.ToString("dd/MM/yyyy")}",
                    Data = new {
                        StartDate = courseStartDate,
                        EndDate = courseEndDate
                    }
                });
            }

            // get weekday schedule id by teaching date request
            var weekday = await _weekDayScheduleService.GetByDateAndCourseId(reqObj.TeachingDate
                , Guid.Parse(reqObj.CourseId));
            // check teaching date exist
            if (weekday is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không thể tạo lịch dạy, Khóa học chỉ diễn ra từ {courseStartDate.ToString("dd/MM/yyyy")}" +
                    $" đến {courseEndDate.ToString("dd/MM/yyyy")}",
                });
            }

            // generate teaching schedule model
            var teachingSchedule = reqObj.ToScheduleModel();
            teachingSchedule.WeekdayScheduleId = weekday.WeekdayScheduleId;

            // check schedule from different course 
            // slot, date, mentorId, !courseId
            var otherCourseSchedule = await _teachingScheduleService.ExistScheduleInOtherCoursesAsync(reqObj.SlotId,
                reqObj.TeachingDate,
                Guid.Parse(reqObj.MentorId),
                Guid.Parse(reqObj.CourseId));
            if (otherCourseSchedule is not null)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Ngày dạy và buổi học đã được đăng ký"
                });
            }

            // check already exist teaching date with particular slot, weekdayscheduleId
            var existSchedule = await _teachingScheduleService.GetByMentorIdAndTeachingDateAsync(
                    weekday.WeekdayScheduleId, Guid.Parse(reqObj.MentorId),
                    reqObj.TeachingDate, reqObj.SlotId);

            if (existSchedule is null)
            {
                // set vehicle
                //teachingSchedule.VehicleId = vehicleId;
                // set schedule status
                teachingSchedule.IsActive = false;
                // create schedule
                var createdSchedule = await _teachingScheduleService.CreateAsync(teachingSchedule);

                if (createdSchedule is not null)
                {

                    // get all weekday of calendar
                    var weekdays = await _weekDayScheduleService.GetAllByCourseId(
                        Guid.Parse(weekday.CourseId));
                    // get all slots 
                    var slots = await _slotService.GetAllAsync();
                    // convert to list of course
                    var listOfSlotSchedule = slots.ToList();
                    // get teaching schedule for each slot
                    foreach (var s in slots)
                    {
                        var teachingSchedules
                            = await _teachingScheduleService.GetBySlotAndWeekDayScheduleAsync(s.SlotId,
                                weekday.WeekdayScheduleId,
                                Guid.Parse(reqObj.MentorId));
                        s.TeachingSchedules = teachingSchedules.ToList();
                    }
                    // set null collection
                    course.Mentors = null;
                    course.FeedBacks = null;
                    course.Curricula = null;
                    course.CoursePackages = null!;

                    // count total teaching schedule
                    var totalSchedule = listOfSlotSchedule.Select(x => x.TeachingSchedules.Count)
                                                          .Count();

                    return new ObjectResult(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status201Created,
                        Data = new
                        {
                            Course = course,
                            Filter = weekdays.Select(x => new
                            {
                                id = x.WeekdayScheduleId,
                                desc = x.WeekdayScheduleDesc
                            }),
                            Weekdays = weekday,
                            TotalSchedule = totalSchedule,
                            SlotSchedules = listOfSlotSchedule
                        }
                    }) { StatusCode = StatusCodes.Status200OK };
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest(new BaseResponse {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = $"Ngày đăng ký và slot đăng ký đã có trong lịch dạy, vui lòng chọn lại"
            });
        }

        /// <summary>
        /// Get all neccesary features to create range schedule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/mentors/{id:Guid}/schedule-register/range")]
        [Authorize(Roles = "Admin,Staff,Mentor")]
        public async Task<IActionResult> TeachingScheduleRegisterRange([FromRoute] Guid id)
        {
            // get course by mentor id
            var courses = await _courseService.GetAllMentorCourseAsync(id);
            // not found
            if (courses is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy người hướng dẫn"
                });
            }

            // get slots
            var slots = await _slotService.GetAllAsync();

            // 404 Not found 
            if (slots is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy buổi học"
                });
            }

            var weekdays = _courseSettings.WeekdaySchedules;

            // 200Ok <- found
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    MentorCourses = courses,
                    Slots = slots,
                    Weekdays = weekdays
                }
            });
        }

        /// <summary>
        /// Register range schedule
        /// </summary>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("staffs/mentors/schedule-register/range")]
        //[Authorize(Roles = "Admin,Staff,Mentor")]
        public async Task<IActionResult> TeachingScheduleRegisterRange([FromBody] TeachingScheduleRangeRequest reqObj)
        {
            // get mentor by id
            var mentor = await _staffService.GetAsync(Guid.Parse(reqObj.MentorId));
            mentor.FeedBacks = null;
            if (mentor is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy người hướng dẫn"
                });
            }

            // get course by id
            var course = await _courseService.GetAsync(Guid.Parse(reqObj.CourseId));
            if (course is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy khóa học"
                });
            }
            // set null collection
            course.Mentors = null;
            course.FeedBacks = null;
            course.Curricula = null;
            course.CoursePackages = null!;

            // course
            var courseMentor = await _courseService.GetByMentorIdAsync(Guid.Parse(reqObj.MentorId));
            if (courseMentor is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy người hướng dẫn trong khóa học"
                });
            }

            // get mentor teaching schedule exist
            var teachingSchedules = await _teachingScheduleService.GetAllByMentorIdAsync(
                Guid.Parse(reqObj.MentorId));
            if (teachingSchedules.Count() > 0)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Người hướng dẫn `{mentor.FirstName} {mentor.LastName}` đã " +
                    $"đăng ký lịch cả khóa cho khóa học `{course.CourseTitle}`"
                });
            }

            // get weekdays schedule by course id
            var weekdaySchedules = await _weekDayScheduleService.GetAllByCourseId(
                Guid.Parse(course.CourseId));

            // get vehicle type by license type id
            var vehicleType = await _vehicleService.GetVehicleTypeByLicenseTypeAsync(
                Convert.ToInt32(course.LicenseTypeId));

            /// Get available vehicle in garage for schedule
            //var vehicle = await _vehicleService.GetVehicleByVehicleTypeAsync(
            //    vehicleType.VehicleTypeId);

            //if(vehicle is null)
            //{
            //    return BadRequest(new BaseResponse { 
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        Message = $"Not found any vehicle type {vehicleType.VehicleTypeDesc} in garage," +
            //        $" Please contact to mananger"
            //    });
            //}

            // update vehicle status
            //await _vehicleService.UpdateActiveStatusAsync(vehicle.VehicleId);

            // check slot exist
            var slots = await _slotService.GetAllAsync();
            if (slots.Count() == 0)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy slot học"
                });
            }



            // generate teaching schedule
            // init model
            var initSchedule = reqObj.ToInitScheduleModel();

            // create range schedule
            bool isSucess = await _teachingScheduleService.CreateRangeBySlotAndWeekdayAsync(reqObj.SlotId, reqObj.Weekdays,
                            weekdaySchedules.First().WeekdayScheduleId,
                            initSchedule);

            /// Get all created schedule
            // course start date
            var courseStartDate = DateTime.ParseExact(Convert.ToDateTime(course.StartDate).ToString("yyyy-MM-dd"),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);

            // get calendar by current date
            var weekday = await _weekDayScheduleService.GetByDateAndCourseId(courseStartDate,
                Guid.Parse(course.CourseId));
            if (weekday is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy lịch"
                });
            }

            // get all weekday of calendar
            var weekdays = await _weekDayScheduleService.GetAllByCourseId(
                Guid.Parse(course.CourseId));

            // convert to list of course
            var listOfSlotSchedule = slots.ToList();

            // get teaching schedule for each slot
            foreach (var s in slots)
            {
                teachingSchedules
                    = await _teachingScheduleService.GetBySlotAndWeekDayScheduleAsync(s.SlotId,
                        weekday.WeekdayScheduleId,
                        Guid.Parse(reqObj.MentorId));
                s.TeachingSchedules = teachingSchedules.ToList();
            }

            // count total teaching schedule
            var totalSchedule = listOfSlotSchedule.Select(x => x.TeachingSchedules.Count)
                                                  .Count();
            // response
            if (isSucess)
            {
                var courseEndDate = courseStartDate.AddMonths(
                    Convert.ToInt32(course.TotalMonth));
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Tạo mới lịch dạy từ ngày {courseStartDate.ToString("dd/MM/yyyy")} " +
                    $"đến ngày {courseEndDate.ToString("dd/MM/yyyy")} thành công",
                    Data = new
                    {
                        Course = course,
                        Filter = weekdays.Select(x => new {
                            Id = x.WeekdayScheduleId,
                            Desc = x.WeekdayScheduleDesc
                        }),
                        Weekdays = weekday,
                        SlotSchedules = listOfSlotSchedule,
                        TotalSchedule = totalSchedule
                    }
                });
            }

            return new ObjectResult(new BaseResponse {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Xảy ra lỗi"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        /// <summary>
        /// Update staff sfeatures
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/update")]
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Do update 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reqObj"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("staffs/{id:Guid}/update")]
        [Authorize(Roles = "Admin")]
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
                    Message = $"Không tìm thấy nhân viên"
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

            // validate birthdate
            var currDate = DateTime.ParseExact(DateTime.Now.ToString(_appSettings.DateFormat),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);
            var birthdate = DateTime.Parse(reqObj.DateBirth);
            if (birthdate >= currDate)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Ngày sinh không hợp lệ"
                });
            }

            // update staff
            var isSucess = await _staffService.UpdateAsync(id, staffModel);

            // update success
            if (isSucess)
            {
                // 200 Ok <- update sucessfully
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Sửa thông tin thành công"
                });
            }

            // 500 Internal <- cause error
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Do delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("staffs/{id:Guid}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStaff([FromRoute] Guid id)
        {
            // get staff
            var staff = await _staffService.GetAsync(id);
            // not exist
            if (staff is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Không tìm thấy nhân viên"
            });

            // delete staff
            bool isSuccess = await _staffService.DeleteAsync(id);
            //// delete account
            //await _accountService.DeleteAsync(staff.Email);
            //// delete address
            //await _addressService.DeleteAsync(Guid.Parse(staff.AddressId));

            if (isSuccess)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Xóa thành công"
                });
            }

            return new ObjectResult(new BaseResponse {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Xóa thật bại"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        /// <summary>
        /// Get all necessary features to import excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("staffs/import-excel")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportExcel()
        {
            // get all jobtitle
            var jobTitles = await _jobTitleService.GetAllAsync();

            if (jobTitles.Count() > 0)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = jobTitles
                });
            }

            return BadRequest(new BaseResponse { 
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Không tìm thấy loại công việc"
            });
        }

        /// <summary>
        /// Import Excel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="jobTitleId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        // import excel
        [HttpPost]
        [Route("staffs/import-excel")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportToExcel(IFormFile file,
            [FromForm] int jobTitleId)
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
                using (var xlPackage = new OfficeOpenXml.ExcelPackage(stream))
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
                        var dateBirth = DateTime.ParseExact(Convert.ToDateTime(worksheet.Cells[row, 5].Value).ToString("dd/MM/yyyy hh:mm:ss"),
                            "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                        var phone = "0" + worksheet.Cells[row, 6].Value.ToString();
                        var street = worksheet.Cells[row, 7].Value.ToString();
                        var district = worksheet.Cells[row, 8].Value.ToString();
                        var city = worksheet.Cells[row, 9].Value.ToString();
                        //var linceseType = worksheet.Cells[row, 10].Value.ToString();

                        // check exist email
                        var existEmail = await _accountService.GetByEmailAsync(email);
                        if (existEmail is not null) return BadRequest(new BaseResponse {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = $"Email của nhân viên {firstName} {lastName}, tại hàng {row} đã tồn tại",
                        });

                        //// get license type by description
                        //var licenseTypeModel = await _licenseTypeService.GetByDescAsync(linceseType.ToUpper());

                        // generate account model
                        var account = new AccountModel
                        {
                            Email = email,
                            Password = PasswordHelper.ConvertToEncrypt(password),
                            IsActive = true,
                            RoleId = jobTitleId
                        };
                        // account validation
                        var accountValidateResult = await account.ValidateAsync();
                        if (accountValidateResult is not null)
                        {
                            return BadRequest(new ErrorResponse
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = $"Xảy ra lỗi tại nhân viên {firstName} {lastName}, dòng {row}",
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
                                Message = $"Xảy ra lỗi tại nhân viên {firstName} {lastName}, hàng {row}",
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
                            //LicenseTypeId = Convert.ToInt32(licenseTypeModel.LicenseTypeId),
                            JobTitleId = jobTitleId
                        };
                        // staff validation
                        var staffValidateResult = await staff.ValidateAsync();
                        if (staffValidateResult is not null)
                        {
                            return BadRequest(new ErrorResponse
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = $"Xảy ra lỗi tại nhân viên {firstName} {lastName}, hàng {row}",
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
                        Message = $"Import excel thành công, tổng {totalStaffs} nhân viên được tạo mới"
                    });
                }
            }
            // Import Failed
            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Import excel không thành công"
            });
        }

        /// <summary>
        /// Export excel
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        // export excel
        [HttpGet]
        [Route("staffs/export-excel")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportToExcel([FromQuery] StaffFilter filters)
        {
            var staffs = await _staffService.GetAllByFilterAsync(filters);
            if (staffs is null)
            {
                return NotFound(new BaseResponse {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy nhân viên để export excel"
                });
            }

            // start exporting to excel
            // create file stream
            var stream = new MemoryStream();

            // create excel package
            using (var xlPackage = new OfficeOpenXml.ExcelPackage(stream))
            {
                // define a worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Staffs");
                // first row
                var startRow = 3;
                // worksheet details
                worksheet.Cells["A1"].Value = "Danh sách nhân viên";
                using (var r = worksheet.Cells["A1:C1"])
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
                //worksheet.Cells["J2"].Value = "License Type";
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
                    //worksheet.Cells[row, 10].Value = s.LicenseType.LicenseTypeDesc;
                    worksheet.Cells[row, 11].Value = s.JobTitle.JobTitleDesc;

                    // next row
                    ++row;

                }
                // properties
                xlPackage.Workbook.Properties.Title = "Danh sách nhân viên";
                xlPackage.Workbook.Properties.Author = "Admin";
                await xlPackage.SaveAsync();

            }
            // read from position
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "staffs.xlsx");
        }
    }
}
