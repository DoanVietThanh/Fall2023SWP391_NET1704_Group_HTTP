using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
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
            // cause error
            if(createdCourse is null) { return StatusCode(StatusCodes.Status500InternalServerError); }

            // response data
            return new ObjectResult(createdCourse) { StatusCode = StatusCodes.Status201Created };
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
