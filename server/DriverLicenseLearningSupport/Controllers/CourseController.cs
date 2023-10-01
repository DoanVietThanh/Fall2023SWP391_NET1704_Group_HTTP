using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IFeedbackService _feedbackService;
        private readonly IStaffService _staffService;
        private readonly IMemberService _memberService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly ICourseServationService _courseReservationService;
        private readonly IVehicleService _vehicleService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly AppSettings _appSettings;
        private readonly IWeekDayScheduleService _weekDayScheduleService;

        public CourseController(ICourseService courseService,
            IFeedbackService feedbackService,
            IStaffService staffService,
            IMemberService memberService,
            IPaymentTypeService paymentTypeService,
            IWeekDayScheduleService weekDayScheduleService,
            ICourseServationService courseReservationService,
            IVehicleService vehicleService,
            ILicenseTypeService licenseTypeService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _courseService = courseService;
            _feedbackService = feedbackService;
            _staffService = staffService;
            _memberService = memberService;
            _paymentTypeService = paymentTypeService;
            _courseReservationService = courseReservationService;
            _vehicleService = vehicleService;
            _licenseTypeService = licenseTypeService;
            _appSettings = monitor.CurrentValue;
            _weekDayScheduleService = weekDayScheduleService;
        }

        [HttpGet]
        [Route("courses/add")]
        public async Task<IActionResult> AddCourse()
        {
            // get all license types
            var licenseTypes = await _licenseTypeService.GetAllAsync();

            // 404 <- not found
            if(licenseTypes is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any license types"
                });
            }

            // 200 OK <- found 
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = licenseTypes
            });
        }

        [HttpPost]
        [Route("courses/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddCourse([FromBody] CourseAddRequest reqObj) 
        {
            // generate course model
            var courseModel = reqObj.ToCourseModel();

            // validation
            var validatioResult = await courseModel.ValidateAsync();
            if(validatioResult is not null)
            {
                return BadRequest(new ErrorResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = validatioResult
                });
            }
        
            // create course 
            var createdCourse = await _courseService.CreateAsync(courseModel);

            if(createdCourse is not null)
            {
                // create course schedule <- from start date to total of month
                var totalMonth = Convert.ToInt32(createdCourse.TotalMonth);
                var startDate = Convert.ToDateTime(createdCourse.StartDate);
                var weekDaySchedules = DateTimeHelper.GenerateRangeWeekday(totalMonth, startDate, 
                    Guid.Parse(createdCourse.CourseId));

                // add range week schedule
                await _weekDayScheduleService.CreateRangeAsync(weekDaySchedules); 
            }

            // cause error
            if (createdCourse is null) { return StatusCode(StatusCodes.Status500InternalServerError); }

            // response data
            return new ObjectResult(createdCourse) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpGet]
        [Route("courses/reservation")]
        public async Task<IActionResult> CourseReservation()
        {
            // get all payment type
            var paymentTypes = await _paymentTypeService.GetAllAsync();
            // 500 Internal <- null <- cause error
            if(paymentTypes is null) { return StatusCode(StatusCodes.Status500InternalServerError); }
            // 200 OK <- found
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = paymentTypes
            });
        }

        [HttpPost]
        [Route("courses/reservation")]
        public async Task<IActionResult> CourseReservation([FromBody] CourseReservationRequest reqObj)
        {
            // check exist member
            var member = await _memberService.GetAsync(reqObj.MemberId);
            if (member is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any member match id {reqObj.MemberId}"
                });
            }
            // check exist mentor 
            var mentor = await _staffService.GetAsync(reqObj.MentorId);
            if (mentor is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any mentor match id {reqObj.MentorId}"
                });
            }
            // check exist course
            var course = await _courseService.GetAsync(reqObj.CourseId);
            if (course is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any member match id {reqObj.CourseId}"
                });
            }

            // generate course reservation model
            var courseReservationModel = reqObj.ToCourseReservationModel();

            // get vehicle for reservation
            var licenseType = await _licenseTypeService.GetByDescAsync("B1");
            //var vehicle = _vehicleService.GetByLicenseTypeAsync(licenseType.LicenseTypeId);

            // gererate current date
            var createDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), 
                _appSettings.DateFormat, CultureInfo.InvariantCulture);

            // current date
            courseReservationModel.CreateDate = createDate;
            // course start date
            courseReservationModel.CourseStartDate = Convert.ToDateTime(course.StartDate);

            // create course reservation
            var createdReservation = await _courseReservationService.CreateAsync(courseReservationModel);

            // create success
            if(createdReservation is not null)
            {
                return new ObjectResult(createdReservation) { StatusCode = StatusCodes.Status201Created };
            }

            // cause error
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        [Route("courses/demo")]
        public async Task<IActionResult> Demo()
        {
            var weekSchedules = DateTimeHelper.GenerateRangeWeekday(2, DateTime.Now, Guid.NewGuid()).ToList();

            var startTime = new TimeSpan(7, 30, 0).ToString(@"hh\:mm");
            var endTime = new TimeSpan(7, 30, 0).Add(TimeSpan.FromHours(2)).ToString(@"hh\:mm");
            var slot1 = new SlotModel {
                SlotId = 1,
                SlotName = "Slot 1",
                Duration = 2,
                Time = new TimeSpan(7, 30, 0),
                SlotDesc = $"{startTime} - {endTime}",
                TeachingSchedules = new List<TeachingScheduleModel>()
            };

            var slot2 = new SlotModel
            {
                SlotId = 2,
                SlotName = "Slot 2",
                Duration = 2,
                Time = new TimeSpan(9, 45, 0),
                SlotDesc = $"{startTime} - {endTime}",
                TeachingSchedules = new List<TeachingScheduleModel>()
            };

            var slot3 = new SlotModel
            {
                SlotId = 3,
                SlotName = "Slot 3",
                Duration = 3,
                Time = new TimeSpan(13, 30, 0),
                SlotDesc = $"{startTime} - {endTime}",
                TeachingSchedules = new List<TeachingScheduleModel>()
            };

            var slot4 = new SlotModel
            {
                SlotId = 4,
                SlotName = "Slot 4",
                Duration = 3,
                Time = new TimeSpan(15, 30, 0),
                SlotDesc = $"{startTime} - {endTime}",
                TeachingSchedules = new List<TeachingScheduleModel>()
            };

            var list = new List<TeachingScheduleModel>();
            var list2 = new List<TeachingScheduleModel>();

            list.Add(new TeachingScheduleModel
            {
                WeekdayScheduleId = weekSchedules[0].WeekdayScheduleId,
                TeachingDate = DateTime.Now.AddDays(2),
                Staff = new StaffModel {
                    StaffId = Guid.NewGuid().ToString(),
                    FirstName = "Le Xuan",
                    LastName = "Phuoc",
                    Address = new AddressModel
                    {
                        Street = "59",
                        District = "GV",
                        City = "HCM"
                    },
                    JobTitle = new JobTitleModel
                    {
                        JobTitleId = 1,
                        JobTitleDesc = "Mentor"
                    }
                },
                SlotId = 1
            });
            list.Add(new TeachingScheduleModel
            {
                WeekdayScheduleId = weekSchedules[0].WeekdayScheduleId,
                TeachingDate = DateTime.Now.AddDays(5),
                Staff = new StaffModel
                {
                    StaffId = Guid.NewGuid().ToString(),
                    FirstName = "Le Xuan",
                    LastName = "Phuoc",
                    Address = new AddressModel
                    {
                        Street = "59",
                        District = "GV",
                        City = "HCM"
                    },
                    JobTitle = new JobTitleModel
                    {
                        JobTitleId = 1,
                        JobTitleDesc = "Mentor"
                    }
                },
                SlotId = 1
            });
            list.Add(new TeachingScheduleModel
            {
                WeekdayScheduleId = weekSchedules[0].WeekdayScheduleId,
                TeachingDate = DateTime.Now.AddDays(6),
                Staff = new StaffModel
                {
                    StaffId = Guid.NewGuid().ToString(),
                    FirstName = "Le Xuan",
                    LastName = "Phuoc",
                    Address = new AddressModel
                    {
                        Street = "59",
                        District = "GV",
                        City = "HCM"
                    },
                    JobTitle = new JobTitleModel
                    {
                        JobTitleId = 1,
                        JobTitleDesc = "Mentor"
                    }
                },
                SlotId = 1
            });
            list.Add(new TeachingScheduleModel
            {
                WeekdayScheduleId = weekSchedules[2].WeekdayScheduleId,
                TeachingDate = DateTime.Now.AddDays(6),
                Staff = new StaffModel
                {
                    StaffId = Guid.NewGuid().ToString(),
                    FirstName = "Le Xuan",
                    LastName = "Phuoc",
                    Address = new AddressModel
                    {
                        Street = "59",
                        District = "GV",
                        City = "HCM"
                    },
                    JobTitle = new JobTitleModel
                    {
                        JobTitleId = 1,
                        JobTitleDesc = "Mentor"
                    }
                },
                SlotId = 1
            });
            list.Add(new TeachingScheduleModel
            {
                WeekdayScheduleId = weekSchedules[4].WeekdayScheduleId,
                TeachingDate = DateTime.Now.AddDays(6),
                Staff = new StaffModel
                {
                    StaffId = Guid.NewGuid().ToString(),
                    FirstName = "Le Xuan",
                    LastName = "Phuoc",
                    Address = new AddressModel
                    {
                        Street = "59",
                        District = "GV",
                        City = "HCM"
                    },
                    JobTitle = new JobTitleModel
                    {
                        JobTitleId = 1,
                        JobTitleDesc = "Mentor"
                    }
                },
                SlotId = 1
            });

            var date = DateTime.Now.AddDays(4).ToString("dd/MM/yyyy");
            var filterDate = DateTime.ParseExact(date, 
                "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var findDate = weekSchedules.Where(x => x.Monday <= filterDate && x.Sunday >= filterDate)
                .FirstOrDefault();

            var weekday = weekSchedules.Where(x => x.WeekdayScheduleId == findDate.WeekdayScheduleId)
                                       .FirstOrDefault();

            var dates = new List<DateTime>() 
            { weekday.Monday, weekday.Tuesday, weekday.Wednesday,
                weekday.Thursday, weekday.Friday, weekday.Saturday, weekday.Sunday};

            // write func for 4 slot
            foreach (var d in dates)
            {
                var schedule = list.Where(x => x.TeachingDate.ToString("dd/MM/yyyy").Equals(
                    d.ToString("dd/MM/yyyy")) && x.SlotId == 1).FirstOrDefault();
                
                if(schedule is not null)
                {
                    slot1.TeachingSchedules.Add(schedule);
                }
                else
                {
                    slot1.TeachingSchedules.Add(null);
                }
            }

            var course = new CourseModel
            {
                CourseId = Guid.NewGuid().ToString(),
                CourseTitle = "Bai lai B1",
                CourseDesc = "Bai lai B1",
            };
            return Ok(new
            {
                Course = course,
                FilterWeekDay = weekSchedules.Select(x => x.WeekdayScheduleDesc),
                Weekdays = weekday,
                Slot1 = slot1
            });
        }

        [HttpGet]
        [Route("courses/{id:Guid}")]
        public async Task<IActionResult> GetCourse([FromRoute] Guid id) 
        {
            var course = await _courseService.GetAsync(id);
            if (course is null) return NotFound(new BaseResponse { 
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any course match id {id}"
            });

            // get all course feeback
            var feedbacks = await _feedbackService.GetAllCourseFeedback(Guid.Parse(course.CourseId));
            course.FeedBacks = feedbacks.ToList();

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = course
            });
        }

        [HttpGet]
        [Route("courses")]
        public async Task<IActionResult> GetAllCourse()
        {
            var courses = await _courseService.GetAllAsync();
            if (courses is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any course"
            });

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = courses
            });
        }

        [HttpGet]
        [Route("courses/hidden")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllHiddenCourse() 
        {
            var courses = await _courseService.GetAllHiddenCourseAsync();
            if (courses is null) 
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Not found any hidden courses"
                });
            }

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = courses
            });
        }

        [HttpPost]
        [Route("courses/mentor/add")]
        public async Task<IActionResult> AddCourseMentor([FromForm] Guid courseId, [FromForm] Guid mentorId)
        {
            // get mentor by id
            var mentor = await _staffService.GetMentorAsync(mentorId);
            // 404 Not Found <- not found any mentor match id
            if (mentor is null) return NotFound(new BaseResponse { 
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any mentor match id {mentorId}"
            });

            // get course by id
            var course = await _courseService.GetAsync(courseId);
            // 404 Not Found <- not found any course match id
            if (course is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any course match id {courseId}"
            });

            // Add mentor to course
            var isSucess = await _courseService.AddMentorAsync(courseId, mentorId);
            // success
            if (isSucess)
            {
                return Ok(new BaseResponse { 
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Add mentor {mentorId} to course {courseId} succesfully"
                });
            }

            // cause error
            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        [HttpPost]
        [Route("courses/curriculum/add")]
        public async Task<IActionResult> AddCourseCurrilum([FromBody] CourseCurriculumAddRequest reqObj) 
        {
            // generate curriculum model
            var curriculumModel = reqObj.ToCurriculumModel();
            // add course curriculum
            var isSuccess = await _courseService.AddCurriculumAsync(reqObj.CourseId,curriculumModel);

            // return <- cause error
            if (!isSuccess) return StatusCode(StatusCodes.Status500InternalServerError);

            // return <- success
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = "Add course curriculum sucessfully"
            });
        }

        [HttpPut]
        [Route("courses/curriculum/{id:int}/update")]
        public async Task<IActionResult> UpdateCourseCurriculum([FromRoute] int id, [FromBody] CourseCurriculumUpdateRequest reqObj) 
        {
            // update course <-> hidden
            var course = await _courseService.GetHiddenCourseAsync(reqObj.CourseId);
            // check curriculum id
            var existCurriculum = course.Curricula.Select(x => x.CurriculumId == id)
                                                  .FirstOrDefault();
            // course not be hidden
            if(course is not null) 
            {
                if (course.IsActive == true) return new ObjectResult(new BaseResponse
                {
                    StatusCode = StatusCodes.Status405MethodNotAllowed,
                    Message = $"Hidden course is required before update course curriculum"
                })
                {
                    StatusCode = StatusCodes.Status405MethodNotAllowed
                };
            }
            // not found
            if (course is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any course match id {reqObj.CourseId}"
                });
            }
            else if (!existCurriculum) 
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any curriculum match id {id} of course {reqObj.CourseId}"
                });
            }

            // generate curriculum model
            var curriculum = reqObj.ToCurriculumModel();
            curriculum.CurriculumId = id;
            // update course curriculum
            var isSucess = await _courseService.UpdateCourseCurriculumAsync(reqObj.CourseId, curriculum);

            // 404 Not Found <- not found curriculum in course 
            if (!isSucess) return BadRequest(new BaseResponse{ 
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found curriculum id {id} in course id {reqObj.CourseId}"
            });
            // 200 OK <- success
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = "Update course curriculum successfully"
            });
        }

        [HttpPut]
        [Route("courses/{id:Guid}/update")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateCourse([FromRoute] Guid id, [FromBody] CourseUpdateRequest reqObj) 
        {
            // update course <-> hidden
            var hiddenCourse = await _courseService.GetHiddenCourseAsync(id);
            if (hiddenCourse is null) return new ObjectResult(new BaseResponse { 
                StatusCode = StatusCodes.Status405MethodNotAllowed,
                Message = $"Hidden course is required before update course"
            }) 
            {
                StatusCode = StatusCodes.Status405MethodNotAllowed
            };


            // generate course model
            var course = reqObj.ToCourseModel();

            // validation
            var validateResult = await course.ValidateAsync();
            if (validateResult is not null) 
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = validateResult
                });
            }

            // update course
            bool isSucess = await _courseService.UpdateAsync(id, course);
            if (!isSucess) 
            { 
                return StatusCode(StatusCodes.Status500InternalServerError); 
            }

            // update sucess
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = $"Update course id {id} successfully"
            });
        }

        [HttpPut]
        [Route("courses/{id:Guid}/hide")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> HideCourse([FromRoute] Guid id) 
        {
            // get hidden course by id 
            var course = await _courseService.GetHiddenCourseAsync(id);
            if (course is not null) return BadRequest(new BaseResponse { 
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"This course is already hidden"
            });

            var isSucess = await _courseService.HideCourseAsync(id);
            if (!isSucess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = $"Hide course id {id} succesfully"
            });
        }

        [HttpPut]
        [Route("courses/{id:Guid}/unhide")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UnhideCourse([FromRoute] Guid id) 
        {
            // get hidden course by id
            var course = await _courseService.GetHiddenCourseAsync(id);
            // not found
            if (course is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any hidden course match id {id}"
            });

            // unhide course <- found
            var isSucess = await _courseService.UnhideAsync(id);
            // 500 Internal <- cause error
            if (!isSucess) return StatusCode(StatusCodes.Status500InternalServerError);
            // 200 Ok <- success
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = $"Unhide course id {id} succesfully"
            });
        }

        [HttpDelete]
        [Route("courses/{id:Guid}/delete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteCourse([FromRoute] Guid id) 
        {
            bool isSucess = await _courseService.DeleteAsync(id);

            if (!isSucess) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = $"Delete course id {id} succesfully"
            });
        }

    }
}
