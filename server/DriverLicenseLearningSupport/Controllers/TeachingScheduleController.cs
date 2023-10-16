using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class TeachingScheduleController : ControllerBase
    {
        private readonly ITeachingScheduleService _teachingScheduleService;
        private readonly IStaffService _staffService;

        public TeachingScheduleController(ITeachingScheduleService teachingScheduleService,
            IStaffService staffService)
        {
            _teachingScheduleService = teachingScheduleService;
            _staffService = staffService;
        }

        [HttpGet]
        [Route("teaching-schedules/{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var teachingSchedule = await _teachingScheduleService.GetAsync(id);
            
            // get staff by id 
            var staff = await _staffService.GetAsync(
                Guid.Parse(teachingSchedule.StaffId));
            // hide feedbacks
            staff.FeedBacks = null!;
            // hide mentors courses
            staff.Courses = null!;

            // set teaching schedule staff info
            teachingSchedule.Staff = staff;
            return Ok(teachingSchedule);
        }
    }
}
