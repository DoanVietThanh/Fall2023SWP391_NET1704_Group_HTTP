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
        private readonly CourseSettings _courseSettings;
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
            IOptionsMonitor<AppSettings> monitor,
            IOptionsMonitor<CourseSettings> courseMonitor)
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
            _courseSettings = courseMonitor.CurrentValue;
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
                    Message = $"Không tìm thấy loại bằng lái nào"
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
            return new ObjectResult(new BaseResponse { 
                StatusCode = StatusCodes.Status201Created,
                Message = "Thêm mới khóa học thành công",
                Data = createdCourse
            }) { StatusCode = StatusCodes.Status201Created };
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
                return new ObjectResult(new BaseResponse
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Thêm mới gói thành công",
                    Data = createdPackage
                })
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }

            return new ObjectResult(new BaseResponse {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Thêm gói thất bại"
            }) {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        [HttpPost]
        [Route("courses/mentor/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddCourseMentor([FromForm] Guid courseId, [FromForm] Guid mentorId)
        {
            // get mentor by id
            var mentor = await _staffService.GetMentorAsync(mentorId);
            // 404 Not Found <- not found any mentor match id
            if (mentor is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Không tìm thấy giảng viên"
            });

            // get course by id
            var course = await _courseService.GetAsync(courseId);
            // 404 Not Found <- not found any course match id
            if (course is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Không tìm thấy khóa học có"
            });

            // already taught this course
            var courseMentor = await _courseService.GetByMentorIdAndCourseIdAsync(mentorId, courseId);
            if (courseMentor is not null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Giảng viên {mentor.FirstName} {mentor.LastName} " +
                    $"đã dạy khóa học này"
                });
            }

            // Add mentor to course
            var isSucess = await _courseService.AddMentorAsync(courseId, mentorId);
            // success
            if (isSucess)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Đăng ký dạy thành công"
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
            var isSuccess = await _courseService.AddCurriculumAsync(reqObj.CourseId, curriculumModel);

            // return <- cause error
            if (!isSuccess) return StatusCode(StatusCodes.Status500InternalServerError);

            // return <- success
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Thêm chương trình giảng dạy thành công"
            });
        }

        [HttpGet]
        [Route("courses/packages/reservation")]
        //[Authorize(Roles = "Admin,Staff")]
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
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CoursePackageReservation([FromBody] CoursePackageReservationRequest reqObj)
        {
            // check exist member
            var member = await _memberService.GetAsync(reqObj.MemberId);
            if (member is null)
            {
                return NotFound(new BaseResponse {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Không tìm thấy thành viên"
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
                    Message = $"Thành viên {member.FirstName} {member.LastName} đã đăng ký khóa học"
                });
            }
            // check exist mentor 
            var mentor = await _staffService.GetAsync(reqObj.MentorId);
            if (mentor is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Không tìm thấy giảng viên"
                });
            }
            else
            {
                var totalMember = await _coursePackageReservationService.GetTotalMemberByMentorIdAsync(reqObj.MentorId);

                if(totalMember >= _courseSettings.TotalMemberOfMentor)
                {
                    return BadRequest(new BaseResponse { 
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = $"Số lượng học viên của giảng viên `{mentor.FirstName} {mentor.LastName}` " +
                        $"đã hết, vui lòng chọn giảng viên khác"
                    });
                }
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

            // generate course reservation model
            var packageReservationModel = reqObj.ToCoursePackageReservationModel();

            // gererate current date
            var createDate = DateTime.ParseExact(DateTime.Now.ToString(_appSettings.DateFormat),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);

            // current date
            packageReservationModel.CreateDate = createDate;

            // create course reservation
            var createdReservation = await _coursePackageReservationService.CreateAsync(
                packageReservationModel);

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
                Message = $"Không tìm thấy khóa học"
            });

            // get course total member
            var courseReservations = await _coursePackageReservationService.GetAllByCourseIdAsync(
                    Guid.Parse(course.CourseId));

            if (course.Mentors is not null)
            {
                foreach (var m in course.Mentors)
                {
                    m.TotalMember = await _coursePackageReservationService.GetTotalMemberByMentorIdAsync(
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
        [Route("courses/packages/{id:Guid}")]
        public async Task<IActionResult> GetCoursePackage([FromRoute] Guid id)
        {
            // get course package by id
            var coursePackage = await _courseService.GetPackageAsync(id);
            // not found
            if (coursePackage is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy gói"
                });
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = coursePackage
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
                Message = $"Không tìm thấy bất kì khóa học nào"
            });

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = courses
            });
        }
      
        [HttpGet]
        [Route("courses/{id:Guid}/members")]
        public async Task<IActionResult> GetAllCourseMemberAsync([FromRoute] Guid id)
        {

            // get course by id
            var course = await _courseService.GetAsync(id);
            if(course is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy khóa học"
                });
            }

            // get all member in course
            var members = await _coursePackageReservationService.GetAllMemberInCourseAsync(id);

            // not found any members
            if (members.Count() == 0)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Không tìm thấy học viên"
                });
            }

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = members
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
                    Message = "Không tìm thấy khóa học bị ẩn"
                });
            }

            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Data = courses
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
                    Message = $"Không tìm thấy gói"
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
                Message = "Thay đổi gói khóa học thành công"
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
                    Message = "Vui lòng ẩn khóa học trước khi thay đổi chương trình giảng dạy"
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
                    Message = $"Không tìm thấy khóa học"
                });
            }
            else if (!existCurriculum) 
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Không tìm thấy chương trình giảng dạy"
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
                Message = $"Không tìm thấy chương trình giảng dạy"
            });
            // 200 OK <- success
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = "Thay đổi chương trình giảng dạy thành công"
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
                Message = $"Vui lòng ẩn khóa học trước khi thay đổi"
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
                Message = $"Thay đổi khóa học thành công"
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
                Message = $"Khóa học này đã được ẩn"
            });

            var isSucess = await _courseService.HideCourseAsync(id);
            if (!isSucess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = $"Ẩn khóa học thành công"
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
                Message = $"Không tìm thấy khóa học"
            });

            // unhide course <- found
            var isSucess = await _courseService.UnhideAsync(id);
            // 500 Internal <- cause error
            if (!isSucess) return StatusCode(StatusCodes.Status500InternalServerError);
            // 200 Ok <- success
            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = $"Hủy ẩn khóa học thành công"
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
                Message = $"Xóa khóa học thành công"
            });
        }


        /// <summary>
        /// Slot management
        /// </summary>
        /// <param name="reqObj"></param>
        /// <returns></returns>
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
                return new ObjectResult(new BaseResponse { 
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Thêm thành công",
                    Data = createdSlot
                }) { StatusCode = StatusCodes.Status201Created };
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
                    Message = "Không tìm thấy slot học nào"
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
