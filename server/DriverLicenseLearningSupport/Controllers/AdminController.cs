using DocumentFormat.OpenXml.Bibliography;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase 
    {
        private readonly IMemberService _memberService;
        private readonly IStaffService _staffService;
        private readonly ICourseService _courseService;
        private readonly ITeachingScheduleService _teachingScheduleService;
        private readonly ICoursePackageReservationService _reservationService;
        private readonly IWeekDayScheduleService _weekdayService;
        private readonly IAccountService _accountService;
        private readonly AppSettings _appSettings;

        public AdminController(IMemberService memberService,
            IStaffService staffService,
            ICourseService courseService,
            ITeachingScheduleService teachingScheduleService,
            ICoursePackageReservationService reservationService,
            IWeekDayScheduleService weekdayService,
            IAccountService accountService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _memberService = memberService;
            _staffService = staffService;
            _courseService = courseService;
            _teachingScheduleService = teachingScheduleService;
            _reservationService = reservationService;
            _weekdayService = weekdayService;
            _accountService = accountService;
            _appSettings = monitor.CurrentValue;
        }

        [HttpGet]
        [Route("admin/accounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccount()
        {
            var accounts = await _accountService.GetAllAsync();

            if(accounts.Count() == 0)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Hiện chưa có tài khoản"
                });
            }

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = accounts
            });
        }

        [HttpPost]
        [Route("admin/accounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountAddRequest reqObj)
        {
            if(reqObj.Email is null || reqObj.Password is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Vui lòng điền đầy đủ thông tin Email, Password"
                });
            }

            // generate account model
            var accountModel = reqObj.ToAccountModel();
            // validation
            var valdiateResult = await accountModel.ValidateAsync();
            if(valdiateResult is not null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = valdiateResult
                });
            }

            // create new account
            var isSucess = await _accountService.CreateAsync(accountModel);

            if (isSucess)
            {
                return Ok(new BaseResponse { 
                    StatusCode = StatusCodes.Status207MultiStatus,
                    Message = "Tạo tài khoản thành công"
                });
            }

            return new ObjectResult(new BaseResponse {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Tạo mới tài khoản thất bại"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        [HttpPut]
        [Route("admin/account/{email}/ban")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BanAccount([FromRoute] string email)
        {
            var account = await _accountService.GetByEmailAsync(email);
            if(account is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Không tìm thấy tài khoản có email {email}"
                });
            }

            var banSucess = await _accountService.BanAccountAsync(email);

            if (banSucess)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status207MultiStatus,
                    Message = "Ẩn tài khoản thành công"
                });
            }

            return new ObjectResult(new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Ẩn tài khoản thất bại"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }


        [HttpPut]
        [Route("admin/account/{email}/unban")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnBanAccount([FromRoute] string email)
        {
            var account = await _accountService.GetByEmailAsync(email);
            if (account is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Không tìm thấy tài khoản có email {email}"
                });
            }

            var unbanSucess = await _accountService.UnBanAccountAsync(email);

            if (unbanSucess)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status207MultiStatus,
                    Message = "Hủy Ẩn tài khoản thành công"
                });
            }

            return new ObjectResult(new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Hủy Ẩn tài khoản thất bại"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        [HttpGet]
        [Route("admin/dashboard")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LoadDashboard()
        {
            // total member
            var members = await _memberService.GetAllAsync();
            var totalMember = members.Count();
            // total staff
            var staffs = await _staffService.GetAllAsync();
            var totalStaff = staffs.Count();
            // total course
            var courses = await _courseService.GetAllAsync();
            var totalCourse = courses.Count();
            // total course package
            var totalCoursePackage = 0;
            foreach(var c in courses)
            {
                var courseModel = await _courseService.GetAsync(Guid.Parse(c.CourseId));
                if(c.CoursePackages.Count > 0)
                {
                    totalCoursePackage += courseModel.CoursePackages.Count;
                }
            }
            // total learning member
            var courseReservations = await _reservationService.GetAllAsync();
            var totalLearningMember = courseReservations.Count();
            // total income
            var totalIncome = courseReservations.Select(x => x.PaymentAmmount).Sum();
            // total income from 1 -> 12
            var monthlyIncomes = DateTimeHelper.GenerateMonthlyIncome(courseReservations.ToList());

            // init curr, prev total slots weekday
            var totalSlotsCurrWeekday = new List<int>();
            var totalSlotsPrevWeekday = new List<int>();

            foreach(var c in courses)
            {
                // total slot pev/curr week
                var weekdays = await _weekdayService.GetAllByCourseId(
                    Guid.Parse(c.CourseId));
                var currDate = DateTime.ParseExact(DateTime.Now.ToString(_appSettings.DateFormat),
                    _appSettings.DateFormat, CultureInfo.InvariantCulture);
                var currWeekId = weekdays.Where(x => currDate >= x.Monday && currDate <= x.Sunday)
                                       .Select(x => x.WeekdayScheduleId)
                                       .FirstOrDefault();
                // get all current week schedules
                var currWeekSchedules = await _teachingScheduleService.GetAllWeekdayScheduleAsync(currWeekId);

                // get prev monday weekdate
                DateTime prevWeekDate = currDate.AddDays(-(int)currDate.DayOfWeek - 6);
                if (((int)currDate.DayOfWeek) == 0) // curr date is sunday
                {
                    // substract 1 more day
                    prevWeekDate = currDate.AddDays(-(int)currDate.DayOfWeek - 6)
                                           .Subtract(TimeSpan.FromDays(1));
                }
                var prevWeekId = weekdays.Where(x => prevWeekDate >= x.Monday && prevWeekDate <= x.Sunday)
                                         .Select(x => x.WeekdayScheduleId)
                                         .FirstOrDefault();
                // get all current week schedules
                var prevWeekSchedules = await _teachingScheduleService.GetAllWeekdayScheduleAsync(prevWeekId);


                // generate collection of curr,prev total slots schedule
                if(totalSlotsCurrWeekday.Count == 0) 
                {
                    totalSlotsCurrWeekday = DateTimeHelper.GenerateWeeklySlots(currWeekSchedules.ToList());
                }
                else
                {
                    totalSlotsCurrWeekday = DateTimeHelper.MultipleWeeklySlots(totalSlotsCurrWeekday,
                        currWeekSchedules.ToList());
                }

                if(totalSlotsPrevWeekday.Count == 0)
                {
                    totalSlotsPrevWeekday = DateTimeHelper.GenerateWeeklySlots(prevWeekSchedules.ToList());
                }
                else
                {
                    totalSlotsPrevWeekday = DateTimeHelper.MultipleWeeklySlots(totalSlotsPrevWeekday,
                        prevWeekSchedules.ToList());
                }
            }

            // total blog
            //var blogs = await _blogService.GetAllAsync();
            //var totalBlog = blogs.Count();
            // statistics by daily date

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    TotalMember = totalMember,
                    TotalCourse = totalCourse,
                    TotalCoursePackage = totalCoursePackage,
                    TotalStaff = totalStaff,
                    TotalCoursePackageRegisterMember = totalLearningMember,
                    TotalIncome = totalIncome,
                    MonthlyIncomes = monthlyIncomes,
                    TotalSlotsCurrWeekday = totalSlotsCurrWeekday,
                    TotalSlotsPrevWeekday = totalSlotsPrevWeekday
                }
            });
        }

    }
}
