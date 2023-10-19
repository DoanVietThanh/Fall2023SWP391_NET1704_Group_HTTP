using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class TeachingScheduleController : ControllerBase
    {
        private readonly ITeachingScheduleService _teachingScheduleService;
        private readonly IStaffService _staffService;
        private readonly ICourseService _courseService;
        private readonly IWeekDayScheduleService _weekDayScheduleService;
        private readonly IRollCallBookService _rollCallBookService;


        public TeachingScheduleController(ITeachingScheduleService teachingScheduleService,
            IRollCallBookService rollCallBookService,
            ICourseService courseService,
            IWeekDayScheduleService weekDayScheduleService,
            IStaffService staffService)
        {
            _teachingScheduleService = teachingScheduleService;
            _staffService = staffService;
            _courseService = courseService;
            _weekDayScheduleService = weekDayScheduleService;
            _rollCallBookService = rollCallBookService;
        }

        [HttpGet]
        [Route("teaching-schedules/{id:int}")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var teachingSchedule = await _teachingScheduleService.GetAsync(id);
            
            if(teachingSchedule is null)
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

            if(rcb is not null)
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
                Data = teachingSchedule
            });
        }
    }
}
