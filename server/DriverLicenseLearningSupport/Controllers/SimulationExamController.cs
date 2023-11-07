using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DriverLicenseLearningSupport.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace DriverLicenseLearningSupport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationExamController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly ISimulationSituationService _simuService;
        private readonly ILicenseTypeService _licenseTypeService;

        public SimulationExamController(IImageService imageService, ISimulationSituationService simuService
            , ILicenseTypeService licenseTypeService)
        {
            _imageService = imageService;
            _simuService = simuService;
            _licenseTypeService = licenseTypeService;
        }

        [HttpGet]
        [Route("/simulation")]
        public async Task<IActionResult> GetAllSimulationSituation()
        {
            var simulationModels = await _simuService.GettAllAsync();
            if (simulationModels is null || simulationModels.Count() == 0)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "không tìm tahasy bài thi mô phỏng nào"
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = simulationModels
                });
            }
        }

        [HttpGet]
        [Route("/simulation/{id:int}")]
        public async Task<IActionResult> GetSimulationQuestionById([FromRoute] int id)
        {
            var simulationQuestion = await _simuService.GetById(id);
            if (simulationQuestion is null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tồn tại câu này"
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = simulationQuestion
                });
            }
        }

        [HttpPost]
        [Route("/simulation")]
        //[Authorize(Roles ="Admin, Staff")]
        public async Task<IActionResult> CreateAsync([FromForm] SimulationAddRequest reqObj)
        {

            //generate model
            var newSimulationModel = reqObj.ToSimulationSituation("", "");

            //check Validation
            var validateResult = await newSimulationModel.ValidateAsync();

            if (validateResult is not null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = validateResult
                });
            }

            //check validaiton of picture input from the request
            var imageFileValidator = new Validation.CreateNewSimulationValidator();
            var ImageValidatorresult = await imageFileValidator.ValidateAsync(reqObj);
            if (!ImageValidatorresult.IsValid) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = ImageValidatorresult.ToProblemDetails()
            });
            //   generate guid id
            var vidId = Guid.NewGuid();
            //upload vid to cloud
            //await _imageService.UploadImageAsync(vidId, reqObj.imageLink);

            //Set vid ID to simulation model 
            newSimulationModel.SimulationVideo = vidId.ToString();

            var imageId = Guid.NewGuid();
            //upload image to cloud
            //await _imageService.UploadImageAsync(imageId, reqObj.imageLink);
            newSimulationModel.ImageResult = imageId.ToString();


            //Create simulation
            var createdSimulation = await _simuService.CreateAsync(newSimulationModel);
            if (createdSimulation is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // Set licenseType to simulation
            createdSimulation.LicenseType = await _licenseTypeService.GetAsync(Convert.ToInt32(createdSimulation.LicenseTypeId));

            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                //Data = createdQuestionModel,
                Data = new
                {
                    simualtionModel = createdSimulation,
                },
                Message = "Create successfully"
            });
        }

        [HttpDelete]
        [Route("/simulation/{id:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleteModel = await _simuService.GetById(id);
            if (deleteModel is null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "không tồn tại câu hỏi mô phỏng tương ứng"
                });
            }
            bool isSuccess = await _simuService.DeleteAsync(id);
            if (isSuccess == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Xóa thành công"
            });
        }

        [HttpPut]
        [Route("/simulation")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateSimulation([FromForm] UpdateSimulationRequest reqObj)
        {
            var updatedSimulation = reqObj.ToSimulationSituationModel();
            if (reqObj.ImageResult != null)
            {
                var newImageResult = Guid.NewGuid();
                updatedSimulation.ImageResult = newImageResult.ToString();
            }
            var updateSimulation = await _simuService.UpdateSimulaitonAsync(updatedSimulation, reqObj.ID);
            if (updateSimulation is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Cap nhat thanh cong"
            });
        }
    }
}

