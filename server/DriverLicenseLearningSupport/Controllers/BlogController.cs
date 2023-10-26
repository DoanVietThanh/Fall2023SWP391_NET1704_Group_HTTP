using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Globalization;

namespace DriverLicenseLearningSupport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly ITagService _tagService;
        private readonly AppSettings _appSettings;

        public BlogController(IBlogService blogService,ITagService tagService, IOptionsMonitor<AppSettings> monitor) 
        {
            _blogService = blogService;
            _tagService = tagService;
            _appSettings = monitor.CurrentValue;
        } 
        
        
        [HttpPost]
        [Route("/blog")]

        public async Task<IActionResult> CreateBlog([FromForm] BlogCreateRequest reqObj) 
        {
            //to Blog Model
            var blog = reqObj.ToBlogModel();
            // Check Blog
            BlogValidator validationRules = new BlogValidator();
            var checkBlogModel = await validationRules.ValidateAsync(blog);
            if (!checkBlogModel.IsValid) 
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = checkBlogModel,
                    Message = "Vui lòng điền lại form"
                });
            }
            //Insert Date
            DateTime StartedDate = DateTime.Now;
            var date = StartedDate.ToString(_appSettings.DateTimeFormat);
            StartedDate = DateTime.ParseExact(date, _appSettings.DateTimeFormat, CultureInfo.InvariantCulture);
            blog.CreateDate = blog.LastModifiedDate = StartedDate;
            
            //Create Tag Model
            List<TagModel> tagModels = new List<TagModel>();
            var tags = reqObj.ToListTagModels();
            
            foreach (var tag in tags) 
            {
                var createdTag = await _tagService.CreateAsync(tag);
                tagModels.Add(createdTag);
            }

            blog.Tags = tagModels;
            var createdBlog = await _blogService.CreateAsync(blog);
            

            if (createdBlog is null)
            {
                return new ObjectResult(new BaseResponse() {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Tạo thất bại"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            else {
                return Ok(new BaseResponse() {
                    StatusCode = StatusCodes.Status200OK,
                    Data = createdBlog,
                    Message ="Tạo thành công"
                });
            }

        }



        //[HttpGet]
        //[Route("/blog")]
        //public async Task<IActionResult> GetAllBlog() 
        //{
        //    var blogs = await _blogService.GetAllAsync();
        //}

    }
}
