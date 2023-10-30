using DocumentFormat.OpenXml.Office2010.Excel;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class TeachingScheduleController : ControllerBase
    {
        private readonly ITeachingScheduleService _teachingScheduleService;
        private readonly IStaffService _staffService;
        private readonly ICourseService _courseService;
        private readonly ISlotService _slotService;
        private readonly IWeekDayScheduleService _weekDayScheduleService;
        private readonly IRollCallBookService _rollCallBookService;
        private readonly IEmailService _emailService;
        private readonly IVehicleService _vehicleService;
        private readonly AppSettings _appSettings;


        public TeachingScheduleController(ITeachingScheduleService teachingScheduleService,
            IRollCallBookService rollCallBookService,
            ICourseService courseService,
            IWeekDayScheduleService weekDayScheduleService,
            ISlotService slotService,
            IStaffService staffService,
            IEmailService emailService,
            IVehicleService vehicleService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _teachingScheduleService = teachingScheduleService;
            _staffService = staffService;
            _courseService = courseService;
            _slotService = slotService;
            _weekDayScheduleService = weekDayScheduleService;
            _rollCallBookService = rollCallBookService;
            _emailService = emailService;
            _vehicleService = vehicleService;
            _appSettings = monitor.CurrentValue;
        }

        /// <summary>
        /// Get teaching schedule by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("teaching-schedules/{id:int}")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var teachingSchedule = await _teachingScheduleService.GetAsync(id);

            if (teachingSchedule is null)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy lịch dạy"
                });
            }

            // get weekday schedule by id
            var weekdaySchedule = await _weekDayScheduleService.GetAsync(
                Convert.ToInt32(teachingSchedule.WeekdayScheduleId));

            // get course by id
            var course = await _courseService.GetAsync(
                Guid.Parse(weekdaySchedule.CourseId));

            // get staff by id 
            var staff = await _staffService.GetAsync(
                Guid.Parse(teachingSchedule.StaffId));
            // hide feedbacks
            staff.FeedBacks = null!;
            // hide mentors courses
            staff.Courses = null!;
            // set teaching schedule staff info
            teachingSchedule.Staff = staff;

            // get rollcallbook
            var rcb = teachingSchedule.RollCallBooks.FirstOrDefault();

            if (rcb is not null)
            {
                // course package
                var coursePackage = await _courseService.GetPackageAsync(
                    Guid.Parse(teachingSchedule.CoursePackageId));

                // get all member rcb
                var rcbooks = await _rollCallBookService.GetAllByMemberIdAsync(
                    Guid.Parse(rcb.Member.MemberId));
                // total hours driven 
                var totalHoursDriven = rcbooks.Select(x => x.TotalHoursDriven).Sum();
                // total km driven
                var totalKmDriven = rcbooks.Select(x => x.TotalKmDriven).Sum();

                var remainingHour = course.TotalHoursRequired - totalHoursDriven;
                var remainingKm = course.TotalKmRequired - totalKmDriven;


                var registeredSession = rcbooks.Count();

                if (registeredSession > 0 && coursePackage?.TotalSession != null)
                {
                    return Ok(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Data = new
                        {
                            TeachingSchedule = teachingSchedule,
                            RemainingRequiredHour = (remainingHour > 0) ? remainingHour : 0,
                            RemainingRequiredKm = (remainingKm > 0) ? remainingKm : 0,
                            RegisteredSession = registeredSession,
                            PackageTotalSession = coursePackage.TotalSession
                        }
                    });
                }

                return Ok(new BaseResponse {
                    StatusCode = StatusCodes.Status200OK,
                    Data = new
                    {
                        TeachingSchedule = teachingSchedule,
                        RemainingRequiredHour = (remainingHour > 0) ? remainingHour : 0,
                        RemainingRequiredKm = (remainingKm > 0) ? remainingKm : 0
                    }
                });
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    TeachingSchedule = teachingSchedule
                }
            });
        }

        /// <summary>
        /// Get all mentors await schedule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("teaching-schedules/await")]
        public async Task<IActionResult> GetAllMentorAwaitSchedule()
        {
            // get members has all await schedule
            var mentors = await _teachingScheduleService.GetAllAwaitScheduleMentor();

            if (mentors.Count() > 0) return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = mentors
            });
            
            return BadRequest(new BaseResponse { 
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Hiện chưa có lịch cần duyệt"
            });
        }

        /// <summary>
        /// Get mentor schedule await details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("teaching-schedules/mentors/{id:Guid}/await-schedule")]
        public async Task<IActionResult> GetAwaitScheduleDetail([FromRoute] Guid id)
        {
            // get mentor by id 
            var mentor = await _staffService.GetAsync(id);
            if (mentor is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy giảng viên"
                });
            }

            // get mentor course
            var course = await _courseService.GetByMentorIdAsync(id);
            if (course is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy khóa học của giảng viên"
                });
            }
            // hide course details
            course.Mentors = null!;
            course.FeedBacks = null!;
            course.Curricula = null!;

            // get first await schedule 
            var firstAwaitSchedule = await _teachingScheduleService.GetFirstAwaitScheduleMentor(id);

            // check current date time with course start date
            var courseStartDate = DateTime.ParseExact(Convert.ToDateTime(course.StartDate).ToString("yyyy-MM-dd"),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);
            var courseTotalMonth = Convert.ToInt32(course.TotalMonth);
            if (firstAwaitSchedule.TeachingDate < courseStartDate &&
               firstAwaitSchedule.TeachingDate > courseStartDate.AddMonths(courseTotalMonth))
            {
                firstAwaitSchedule.TeachingDate = courseStartDate;
            }

            // get calendar by current date
            var weekday = await _weekDayScheduleService.GetByDateAndCourseId(firstAwaitSchedule.TeachingDate,
                Guid.Parse(course.CourseId));
            if (weekday is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Giảng viên chưa có lịch dạy nào"
                });
            }
            // get all weekday of calendar
            var weekdays = await _weekDayScheduleService.GetAllByCourseId(
                Guid.Parse(course.CourseId));
            // get all slots 
            var slots = await _slotService.GetAllAsync();
            // convert to list of course
            var listOfSlotSchedule = slots.ToList();

            // get all teaching schedule of mentor
            var filterSchedules = await _teachingScheduleService.GetAllByMentorIdAsync(id);
            var mentorVehicle = filterSchedules.Select(x => x.Vehicle).FirstOrDefault();

            // get teaching schedule for each slot
            foreach (var s in slots)
            {
                var teachingSchedules
                    = await _teachingScheduleService.GetAllAwaitScheduleByMentorAsync(s.SlotId,
                        weekday.WeekdayScheduleId, id);
                s.TeachingSchedules = teachingSchedules.ToList();
            }


            // get mentor schedule vehicle
            VehicleModel mentorScheduleVehicle = null!;
            var filtersSchedule = await _teachingScheduleService.GetAllByMentorIdAsync(id);
            if (filtersSchedule.Count() > 0)
            {
                mentorScheduleVehicle = filtersSchedule.Select(x => x.Vehicle).FirstOrDefault();
            }

            // get vehicle type
            var vehicleType = await _vehicleService.GetVehicleTypeByLicenseTypeAsync(
                Convert.ToInt32(course.LicenseTypeId));
            // get all active vehicle by vehicle type
            var activeVehicles = await _vehicleService.GetAllActiveVehicleByType(vehicleType.VehicleTypeId);
            // get all inactive vehicle by vehicle type
            var inactiveVehicles = await _vehicleService.GetAllInActiveVehicleByType(vehicleType.VehicleTypeId);

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
                    SlotSchedules = listOfSlotSchedule,
                    ActiveVehicles = activeVehicles,
                    MentorScheduleVehicle = mentorScheduleVehicle,
                    TotalInActiveVehicle = inactiveVehicles.Count()
                }
            });
        }

        [HttpGet]
        [Route("teaching-schedules/mentors/{id:Guid}/await-schedule/filter")]
        public async Task<IActionResult> GetAwaitScheduleDetail([FromRoute] Guid id,
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
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = $"Not found any schedule match date {filters.TeachingDate}"
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
                    = await _teachingScheduleService.GetAllAwaitScheduleByMentorAsync(s.SlotId,
                        weekday.WeekdayScheduleId, id);
                s.TeachingSchedules = teachingSchedules.ToList();
            }

            // get mentor schedule vehicle
            VehicleModel mentorScheduleVehicle = null!;
            var filtersSchedule = await _teachingScheduleService.GetAllByMentorIdAsync(id);
            if(filtersSchedule.Count() > 0)
            {
                mentorScheduleVehicle = filtersSchedule.Select(x => x.Vehicle).FirstOrDefault();
            }

            // get course by id 
            var course = await _courseService.GetAsync(Guid.Parse(weekday.CourseId));
            course.Mentors = null;
            course.FeedBacks = null;
            course.Curricula = null;

            // get vehicle type
            var vehicleType = await _vehicleService.GetVehicleTypeByLicenseTypeAsync(
                Convert.ToInt32(course.LicenseTypeId));
            // get all active vehicle by vehicle type
            var activeVehicles = await _vehicleService.GetAllActiveVehicleByType(vehicleType.VehicleTypeId);
            // get all inactive vehicle by vehicle type
            var inactiveVehicles = await _vehicleService.GetAllInActiveVehicleByType(vehicleType.VehicleTypeId);

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
                    SlotSchedules = listOfSlotSchedule,
                    ActiveVehicles = activeVehicles,
                    MentorScheduleVehicle = mentorScheduleVehicle,
                    TotalInActiveVehicle = inactiveVehicles.Count()
                }
            });
        }
        
        /// <summary>
        /// Approve await schedule by mentor id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("teaching-schedule/mentors/{id:Guid}/approve")]
        public async Task<IActionResult> BrowseAwaitSchedule([FromRoute] Guid id, int vehicleId)
        {
            // get vehicle by id
            var vehicle = await _vehicleService.GetAsync(vehicleId);
            if (vehicle is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy phương tiện"
                });
            }

            // get mentor by id
            var mentor = await _staffService.GetAsync(id);
            if (mentor is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy giảng viên"
                });
            }

            // course
            var course = await _courseService.GetByMentorIdAsync(id);
            if(course is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Người hướng dẫn chưa được cấp phép dạy khóa học"
                });
            }

            // course startdate
            var date = Convert.ToDateTime(course.StartDate);
            // course total month
            var totalMonth = Convert.ToInt32(course.TotalMonth);

            // update schedule status
            bool isApproved = await _teachingScheduleService.ApproveMentorAwaitSchedule(id);
            // send schedule approval email
            var message = new EmailMessage(new string[] { mentor.Email! }, "Lịch dạy đăng ký đã " +
                "được duyệt thành công", $"Lịch dạy '{course.CourseTitle}' từ ngày " +
                $"{date.ToString("dd/MM/yyyy")}" +
                $" đến {date.AddMonths(totalMonth).ToString("dd/MM/yyyy")} đã được duyệt thành công. \n " +
                $"Mọi thắc mắc xin liên hệ để được điều chỉnh sớm nhất \n" +
                $"Xin cảm ơn.");
           //_emailService.SendEmail(message);

            if (isApproved)
            {
                await _teachingScheduleService.AddRangeVehicleMentorSchedule(id, vehicleId);
               await _vehicleService.UpdateActiveStatusAsync(vehicleId);
                return Ok(new BaseResponse {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Duyệt lịch học thành công"
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
        /// Deny await schedule by mentor id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("teaching-schedule/mentors/{id:Guid}/deny")]
        public async Task<IActionResult> DenyAwaitSchedule([FromRoute] Guid id,
            string message)
        {

            if (String.IsNullOrEmpty(message))
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Vui lòng điền lý do từ chối"
                });
            }

            // get mentor by id
            var mentor = await _staffService.GetAsync(id);
            if (mentor is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy giảng viên"
                });
            }

            // course
            var course = await _courseService.GetByMentorIdAsync(id);

            // update status
            await _teachingScheduleService.DenyMentorAwaitSchedule(id);

            // course startdate
            var date = Convert.ToDateTime(course.StartDate);
            // course total month
            var totalMonth = Convert.ToInt32(course.TotalMonth);

            // send schedule deny email
            var emailMessage = new EmailMessage(new string[] { mentor.Email! },"Đăng ký lịch dạy đã bị từ chối",
                $"Lịch dạy '{course.CourseTitle}' từ ngày " +
                $"{date.ToString("dd/MM/yyyy")} đến {date.AddMonths(totalMonth).ToString("dd/MM/yyyy")} \n" + 
                $"Lý do từ chối: \n" +
                message + "\n Xin cảm ơn.");
            //_emailService.SendEmail(emailMessage);

            if (emailMessage is not null)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Từ chối lịch học thành công"
                });
            }

            return new ObjectResult(new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Xảy ra lỗi"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }


        /// <summary>
        /// Substitute teaching schedule with other mentor
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>s
        //[HttpGet]
        //[Route("teaching-schedule/substitute")]
        //public async Task<IActionResult> SubstituteSchedule()
        //{

        //}
    }
}
