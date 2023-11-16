using DocumentFormat.OpenXml.Drawing;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.Validation;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web;

namespace DriverLicenseLearningSupport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly ITagService _tagService;
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _memoryCache;
        private readonly AppSettingsConfig _appSettingConfig;
        private readonly ICommentService _commentService;
        private readonly IStaffService _staffService;

        public BlogController(IBlogService blogService, ITagService tagService, IOptionsMonitor<AppSettings> monitor, IMemoryCache memoryCache, IOptionsMonitor<AppSettingsConfig> monitor1, ICommentService commentService, IStaffService staffService)
        {
            _blogService = blogService;
            _tagService = tagService;
            _appSettings = monitor.CurrentValue;
            _memoryCache = memoryCache;
            _appSettingConfig = monitor1.CurrentValue;
            _commentService = commentService;
            _staffService = staffService;
        }
        [HttpGet]
        [Route("/blog/tags")]
        public async Task<IActionResult> GetAllTagAsync()
        {
            var tags = await _tagService.GetAllAsync();
            if (tags == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Không có thẻ nào",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new BaseResponse
            {
                Message = "Tải thành công",
                StatusCode = StatusCodes.Status200OK,
                Data = tags
            });
        }


        #region Tạo blog
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
            if (reqObj.Image is not null)
            {
                //check validaiton of picture input from the request
                var imageFileValidator = new Validation.CreateNewBlogValidator();
                var ImageValidatorresult = await imageFileValidator.ValidateAsync(reqObj);
                if (!ImageValidatorresult.IsValid) return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ImageValidatorresult.ToProblemDetails()
                });
                //   generate guid id
                var imageId = Guid.NewGuid();
                //upload image to cloud
                //await _imageService.UploadImageAsync(imageId, reqObj.imageLink);

                //Set image Id to question model 
                blog.Image = imageId.ToString();
            }

            //Insert Date
            DateTime StartedDate = DateTime.Now;
            var date = StartedDate.ToString(_appSettings.DateTimeFormat);
            StartedDate = DateTime.ParseExact(date, _appSettings.DateTimeFormat, CultureInfo.InvariantCulture);
            blog.CreateDate = blog.LastModifiedDate = StartedDate;

            //Create Tag Model
            List<TagModel> tagModels = new List<TagModel>();

            foreach (int id in reqObj.TagIds)
            {
                var createdTag = await _tagService.GetTagById(id);
                tagModels.Add(createdTag);
            }

            blog.Tags = tagModels.ToList();
            var createdBlog = await _blogService.CreateAsync(blog);

            //Get StaffInf
            Guid Sid = Guid.Parse(reqObj.StaffId);
            var staffInfo = await _staffService.GetAsync(Sid);
            if (staffInfo is null)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy người đăng"
                });
            }

            if (createdBlog is null)
            {
                return new ObjectResult(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Tạo thất bại"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            else
            {

                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = new
                    {
                        blogs = createdBlog,
                    },
                    Message = "Tạo thành công"
                }); ;
            }

        }
        #endregion

        #region Show toàn bộ blog không comt
        [HttpGet]
        [Route("/blog")]
        public async Task<IActionResult> GetAllBlog()
        {
            var blogs = await _blogService.GetAllBlogWithoutCmt();
            if (blogs is null)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Không có bài đăng",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Data = blogs,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Tải bài đăng thành công"
                });
            }
        }
        #endregion

        [HttpGet]
        [Route("/blog/{page:int}")]
        public async Task<IActionResult> GetAllBlogWithPage([FromRoute] int page = 1)
        {
            if (!_memoryCache.TryGetValue(_appSettings.TheoryCacheKey,
                out IEnumerable<BlogModel> blogs))
            {

                blogs = await _blogService.GetAllBlogWithoutCmt();


                // cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // none access exceeds 45s <- remove cache
                    .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                    // after 10m from first access <- remove cache
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(600))
                    // cache priority
                    .SetPriority(CacheItemPriority.Normal);
                // set cache

                _memoryCache.Set(_appSettings.MembersCacheKey, blogs, cacheEntryOptions);
            }
            else
            {
                blogs = (IEnumerable<BlogModel>)_memoryCache.Get(_appSettings.PageSize);
            }
            int pageSize = _appSettings.PageSize;

            var list = PaginatedList<BlogModel>.CreateByIEnumerable(blogs, page, pageSize);
            if (blogs is null)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Không có bài đăng",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Data = new
                    {
                        blogs = blogs,
                        TotalPage = list.TotalPage,
                        PageIndex = list.PageIndex
                    },
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Tải bài đăng thành công"
                });
            }
        }

        #region Xem blog cá nhân
        [HttpGet]
        [Route("/blog/{staffId:Guid}")]
        public async Task<IActionResult> GetBlogByStaffId([FromRoute] Guid StaffId)
        {
            var blogs = await _blogService.GetBlogsByStaffId(StaffId);
            if (blogs is null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Không tìm thấy blog",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new BaseResponse
            {
                Message = "Tải bài đăng cá nhân thành công",
                StatusCode = StatusCodes.Status200OK,
                Data = blogs
            });
        }
        #endregion

        [HttpPut]
        [Route("/blog")]
        public async Task<IActionResult> UpdateBlogAsync([FromForm] UpdateBlogRequest reqObj)
        {
            BlogModel blog = new BlogModel();
            if (reqObj.Image != null)
            {
                //check validaiton of picture input from the request
                var imageFileValidator = new Validation.UpdateBlogValidator();
                var ImageValidatorresult = await imageFileValidator.ValidateAsync(reqObj);
                if (!ImageValidatorresult.IsValid) return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ImageValidatorresult.ToProblemDetails()
                });
                //   generate guid id
                var imageId = Guid.NewGuid();
                //upload image to cloud
                //await _imageService.UploadImageAsync(imageId, reqObj.imageLink);

                //Set image Id to question model 
                blog.Image = imageId.ToString();
            }
            blog.BlogId = reqObj.BlogId;
            blog.Content = HttpUtility.HtmlEncode(reqObj.Content);
            blog.Title = HttpUtility.HtmlEncode(reqObj.Title);
            DateTime lastModifiedDate = DateTime.UtcNow;

            List<TagModel> tagModels = new List<TagModel>();

            foreach (int id in reqObj.TagIds)
            {
                var createdTag = await _tagService.GetTagById(id);
                tagModels.Add(createdTag);
            }
            blog.Tags = tagModels;

            bool isSuccess = await _blogService.UpdateBlogAsync(blog);
            if (isSuccess)
            {
                return Ok(new BaseResponse
                {
                    Message = "Cập nhật thành công",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Đã xảy ra lỗi"
                });
            }
        }

        [HttpDelete]
        [Route("/blog/{blogId:int}")]
        public async Task<IActionResult> DeleteBlogAsync(int blogId)
        {
            var blog = await _blogService.GetBlogByIdAsync(blogId);
            if (blog == null)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy Blog"
                });
            }
            bool isSuccess = await _blogService.DeleteBlogAsync(blogId);
            if (isSuccess)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Xóa thành công"
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Đã xảy ra lỗi"
                });
            }
        }

        #region Search theo tên tác giả hoặc tựa đề
        [HttpGet]
        [Route("/blog/search/{searchString}")]
        public async Task<IActionResult> SearchBlogAsync(string searchString)
        {
            var searchBlogsByAuthor = await _blogService.SearchBlogByAuthorAsync(searchString);
            var searchBlogsByTitle = await _blogService.SearchBlogByNameAsync(searchString);
            var searchBlogs = searchBlogsByAuthor.Union(searchBlogsByTitle);
            if (searchBlogs is null || searchBlogs.Count() == 0)
            {
                return NotFound(new ErrorResponse()
                {
                    Message = "Kết quả tìm kiếm không có",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Message = "Tìm kiếm thành công",
                    StatusCode = StatusCodes.Status200OK,
                    Data = searchBlogs
                });
            }
        }
        #endregion

        [HttpGet]
        [Route("/blog/blog_id/{id:int}")]
        public async Task<IActionResult> GetBlogById([FromRoute] int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog is null)
            {
                return NotFound(new ErrorResponse()
                {
                    Message = "Không tìm thấy blog",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            else
            {
                //var configList = _appSettingConfig.BlogSettings.BlogAccessData.ToList();
                //var comments = await _commentService.GetAllInBlogAsync(id);
                //int countCmt = comments.Count();
                //var countList = _appSettingConfig.BlogSettings.BlogAccessData.ToList();
                //BlogCountModel model = countList.FirstOrDefault(x => x.BlogId == id);
                //if (model is null)
                //{
                //    model = new BlogCountModel
                //    {
                //        BlogId = id,
                //        Comment = countCmt > 0 ? countCmt : 0,
                //        View = model.View + 1
                //    };
                //}

                //string json = JsonSerializer.Serialize();
                //await System.IO.File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "test.json"), json);


                return Ok(new BaseResponse()
                {
                    Message = "Thành công",
                    StatusCode = StatusCodes.Status200OK,
                    Data = blog
                });
            }
        }
        [HttpGet]
        [Route("/blog/tags/{id:int}")]
        public async Task<IActionResult> SearchByTagId([FromRoute] int id)
        {
            var blogs = await _blogService.SearchBlogByTagId(id);
            if (blogs is null)
            {
                return NotFound(new ErrorResponse()
                {
                    Message = "Không có bài viết nào cho thẻ",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            else
            {

                return Ok(new BaseResponse()
                {
                    Message = "Tải thành công",
                    StatusCode = StatusCodes.Status200OK,
                    Data = blogs
                });
            }
        }
    }
}
