using Amazon.Runtime.Internal.Util;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DriverLicenseLearningSupport.Entities;
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
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Security.Certificates;
using System;
using System.Data;
using System.Globalization;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
        private readonly ICoursePackageReservationService _coursePackageReservationService;
        private readonly IVehicleService _vehicleService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly ISlotService _slotService;
        private readonly AppSettings _appSettings;
        private readonly IWeekDayScheduleService _weekDayScheduleService;

        public CourseController(ICourseService courseService,
            IFeedbackService feedbackService,
            IStaffService staffService,
            IMemberService memberService,
            IPaymentTypeService paymentTypeService,
            IWeekDayScheduleService weekDayScheduleService,
            ICoursePackageReservationService coursePackageReservationService,
            IVehicleService vehicleService,
            ILicenseTypeService licenseTypeService,
            ISlotService slotService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _courseService = courseService;
            _feedbackService = feedbackService;
            _staffService = staffService;
            _memberService = memberService;
            _paymentTypeService = paymentTypeService;
            _coursePackageReservationService = coursePackageReservationService;
            _vehicleService = vehicleService;
            _licenseTypeService = licenseTypeService;
            _slotService = slotService;
            _appSettings = monitor.CurrentValue;
            _weekDayScheduleService = weekDayScheduleService;
        }

        [HttpGet]
        [Route("courses/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddCourse()
        {
            // get all license types
            var licenseTypes = await _licenseTypeService.GetAllAsync();

            // 404 <- not found
            if (licenseTypes is null)
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
            if (validatioResult is not null)
            {
                return BadRequest(new ErrorResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = validatioResult
                });
            }

            // create course 
            var createdCourse = await _courseService.CreateAsync(courseModel);

            if (createdCourse is not null)
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
        [Route("courses/packages/reservation")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CoursePackageReservation()
        {
            // get all payment type
            var paymentTypes = await _paymentTypeService.GetAllAsync();
            // 500 Internal <- null <- cause error
            if (paymentTypes is null) { return StatusCode(StatusCodes.Status500InternalServerError); }
            // 200 OK <- found
            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Data = paymentTypes
            });
        }

        [HttpPost]
        [Route("courses/packages/reservation")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CoursePackageReservation([FromBody] CoursePackageReservationRequest reqObj)
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
            var coursePackage = await _courseService.GetPackageAsync(reqObj.CoursePackageId);
            if (coursePackage is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any course package match id {reqObj.CoursePackageId}"
                });
            }
            // check member already reservation
            var PackageReservation = await _coursePackageReservationService.GetByMemberAsync(reqObj.MemberId);
            if (PackageReservation is not null)
            {
                // get course by course package id
                var course = await _courseService.GetAsync(
                    Guid.Parse(PackageReservation.CoursePackage.CourseId));

                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Member '{member.FirstName} {member.LastName}' already " +
                    $"reservation in Course '{course.CourseTitle}'," +
                    $" Member just learn one course only"
                });
            }

            // generate course reservation model
            var packageReservationModel = reqObj.ToCoursePackageReservationModel();

            // get vehicle for reservation
            var licenseType = await _licenseTypeService.GetAsync(
                Convert.ToInt32(coursePackage.Course.LicenseTypeId));
            var vehicle = await _vehicleService.GetByLicenseTypeIdAsync(licenseType.LicenseTypeId);

            // check vehicle exist
            if (vehicle is null)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any vehicles in garage with license type {licenseType.LicenseTypeDesc}"
                });
            }

            // set vehicle for course reservation
            packageReservationModel.VehicleId = vehicle.VehicleId;

            // gererate current date
            var createDate = DateTime.ParseExact(DateTime.Now.ToString(_appSettings.DateFormat),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);

            // current date
            packageReservationModel.CreateDate = createDate;

            // create course reservation
            var createdReservation = await _coursePackageReservationService.CreateAsync(
                packageReservationModel);
            createdReservation.Vehicle = vehicle;

            // payment type
            var paymentType = await _paymentTypeService.GetAsync(reqObj.PaymentTypeId);
            if (paymentType.PaymentTypeId == 1)
            {
                return new ObjectResult(new BaseResponse {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Bạn đã đăng ký thành công, vui lòng " +
                    "đến trung tâm thanh toán để được xếp lịch sớm nhất"
                }) { StatusCode = StatusCodes.Status201Created };
            }
            else if (paymentType.PaymentTypeId == 3)
            {
                return new ObjectResult(new BaseResponse
                {
                    StatusCode = StatusCodes.Status201Created,
                    Data = new
                    {
                        PaymentContent = $"Thanh toán {coursePackage.Course.CourseTitle}",
                        PaymentCurrency = "VND",
                        CourseReservationId = createdReservation.CoursePackageReservationId,
                        RequiredAmount = Convert.ToDecimal(coursePackage.Cost),
                        PaymentLanguage = "vn",
                        MemberId = createdReservation.MemberId,
                        PaymentTypeDesc = paymentType.PaymentTypeDesc,
                        Signature = Guid.NewGuid().ToString()
                    }
                })
                { StatusCode = StatusCodes.Status201Created };
            }

            // create success
            if (createdReservation is not null)
            {
                return new ObjectResult(createdReservation) { StatusCode = StatusCodes.Status201Created };
            }

            // cause error
            return StatusCode(StatusCodes.Status500InternalServerError);
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

            // get course total member
            var courseReservations = await _coursePackageReservationService.GetAllByCourseId(
                    Guid.Parse(course.CourseId));

            if (course.Mentors is not null)
            {
                foreach (var m in course.Mentors)
                {
                    m.TotalMember = await _coursePackageReservationService.GetTotalMemberByMentorId(
                        Guid.Parse(m.StaffId));
                }
            }

            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Data = new {
                    Course = course,
                    Mentors = course.Mentors.Select(x => new
                    {
                        MentorId = x.StaffId,
                        MentorName = $"{x.FirstName} {x.LastName}"
                    }),
                    TotalMember = courseReservations is not null
                    ? courseReservations.Count() : 0,
                }
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
        [Route("courses/{id:Guid}/packages/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddCoursePackage([FromRoute] Guid id,
            [FromBody] CoursePackageAddRequest reqObj)
        {
            // get course by id 
            var course = await _courseService.GetAsync(id);
            if(course is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any course match id {id}"
                });
            }
            // generate package model
            var packageModel = reqObj.ToCoursePackageModel();
            // init course package id
            packageModel.CoursePackageId = Guid.NewGuid().ToString();
            packageModel.CourseId = course.CourseId;
            // validation
            var packageValidateResult = await packageModel.ValidateAsync();
            if(packageValidateResult is not null)
            {
                return BadRequest(new ErrorResponse{ 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = packageValidateResult
                });
            }
            // create package
            var createdPackage = await _courseService.CreatePackageAsync(packageModel);
            // response
            if(createdPackage is not null)
            {
                return new ObjectResult(createdPackage)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        [Route("courses/packages/{id:Guid}")]
        public async Task<IActionResult> GetCoursePackage([FromRoute] Guid id)
        {
            // get course package by id
            var coursePackage = await _courseService.GetPackageAsync(id);
            // not found
            if(coursePackage is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any package match id {id}"
                });
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = coursePackage
            });
        }

        [HttpPut]
        [Route("courses/packages/{id:Guid}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateCoursePackage([FromRoute] Guid id,
            [FromBody] CoursePackageUpdateRequest reqObj)
        {
            // get package by id
            var package = await _courseService.GetPackageAsync(id);
            if(package is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any course package {id}"
                });
            }
            // generate model
            var packageModel = reqObj.ToCoursePackageModel();
            // validation
            var packageValidateResult = await packageModel.ValidateAsync();
            if(packageValidateResult is not null)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = packageValidateResult
                });
            }
            // update model
            bool isSucess = await _courseService.UpdatePackageAsync(id, packageModel);
            // response
            if (isSucess) return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Update course package successfully"
            });

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        //[HttpDelete]
        //[Route("courses/packages/{id:Guid}")]
        //public async Task<IActionResult> DeleteCoursePackage([FromRoute] Guid id) 
        //{
        //    var coursePackage = await _courseService.GetPackageAsync(id);
        //    if(coursePackage is null)
        //    {
        //        return BadRequest(new BaseResponse
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Message = $"Not found any package match id {id}"
        //        });
        //    }

        //    // delete async
        //    bool isSucess = await _courseService.DeletePackageAsync(id);

        //    if (isSucess)
        //    {
        //        return Ok(new BaseResponse
        //        {
        //            StatusCode = StatusCodes.Status200OK,
        //            Message = $"Delete course package {id} success"
        //        });
        //    }

        //    return StatusCode(StatusCodes.Status500InternalServerError);
        //}

        [HttpPost]
        [Route("courses/mentor/add")]
        [Authorize(Roles = "Admin,Staff")]
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

            // already taught this course
            var courseMentor = await _courseService.GetByMentorIdAndCourseIdAsync(mentorId, courseId);
            if(courseMentor is not null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Mentor {mentorId} already taught course {courseId}"
                });
            }
            
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
        [Authorize(Roles = "Admin,Staff")]
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
        [Route("courses/curriculum/{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
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
        [Route("courses/{id:Guid}")]
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
        [Route("courses/{id:Guid}")]
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

        // Slot management
        [HttpPost]
        [Route("courses/slot")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddSlot([FromBody] SlotAddRequest reqObj)
        {
            // generate slot model
            var slot = reqObj.ToSlotModel();
            var time = TimeSpan.Parse(slot.Time.ToString());
            // generate slot desc
            var startTime = time.ToString(@"hh\:mm");
            var endTime = time.Add(TimeSpan.FromHours(2)).ToString(@"hh\:mm");
            // set slot desc
            slot.SlotDesc = $"{startTime} - {endTime}";

            // create slot 
            var createdSlot = await _slotService.CreateAsync(slot);

            if(createdSlot is not null)
            {
                return new ObjectResult(createdSlot) { StatusCode = StatusCodes.Status201Created };
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        [Route("courses/slot")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllSlot()
        {
            var slots = await _slotService.GetAllAsync();
            if(slots.Count() == 0)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Not found anys slots"
                });
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Data = slots
            });
        }
    }
}
