using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Org.BouncyCastle.Security;
using System.Globalization;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class MemberController : ControllerBase
    {
        // DateTime format
        //public static string dateFormat = "yyyy-MM-dd";
        // Default Avatar
        //public static string defaultAvatar = "a42e811d-d22b-4ede-b955-1437ebaeeb9d";
        // Dependency Injection
        private readonly IMemberService _memberService;
        private readonly ILicenseTypeService _licenseTypes;
        private readonly ILicenseRegisterFormService _licenseRegisterFormService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IStaffService _staffService;
        private readonly ICourseService _courseService;
        private readonly IFeedbackService _feedbackService;
        private readonly IImageService _imageService;
        private readonly ICoursePackageReservationService _courseReservationService;
        private readonly ITeachingScheduleService _teachingScheduleService;
        private readonly IWeekDayScheduleService _weekDayScheduleService;
        private readonly IRollCallBookService _rollCallBookService;
        private readonly ISlotService _slotService;
        private readonly IMemoryCache _memoryCache;
        private readonly AppSettings _appSettings;
        // cache key
        //private readonly static string _cacheKey = "MembersCacheKey";
        public MemberController(IMemberService memberService,
            ILicenseTypeService licenseType,
            IMemoryCache memoryCache,
            ILicenseTypeService licenseTypeService,
            IAddressService addressService,
            IAccountService accountService,
            IRoleService roleService,
            ILicenseRegisterFormService licenseRegisterFormService,
            IImageService imageService,
            IStaffService staffService,
            IFeedbackService feedbackService,
            ICourseService courseService,
            ICoursePackageReservationService courseReservationService,
            ITeachingScheduleService teachingScheduleService,
            IWeekDayScheduleService weekDayScheduleService,
            ISlotService slotService,
            IRollCallBookService rollCallBookService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _memberService = memberService;
            _licenseTypes = licenseType;
            _memoryCache = memoryCache;
            _licenseTypeService = licenseTypeService;
            _addressService = addressService;
            _accountService = accountService;
            _roleService = roleService;
            _staffService = staffService;
            _courseService = courseService;
            _feedbackService = feedbackService;
            _licenseRegisterFormService = licenseRegisterFormService;
            _imageService = imageService;
            _courseReservationService = courseReservationService;
            _teachingScheduleService = teachingScheduleService;
            _weekDayScheduleService = weekDayScheduleService;
            _rollCallBookService = rollCallBookService;
            _slotService = slotService;
            _appSettings = monitor.CurrentValue;
        }


        // Member Management
        [HttpGet]
        [Route("members/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddMember()
        {
            // get all license types 
            var licenseTypes = await _licenseTypeService.GetAllAsync();
            // response
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new { LicenseTypes = licenseTypes }
            });
        }

        [HttpPost]
        [Route("members/add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddMember([FromBody] MemberAddRequest reqObj)
        {

            // check email already exist
            var existEmail = await _memberService.GetByEmailAsync(reqObj.Email);
            if (existEmail is not null) return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = $"Email {reqObj.Email} already exist"
            });

            // generate account model
            var account = reqObj.ToAccountModel();
            // validate account model
            var accountValidateResult = await account.ValidateAsync();
            if (accountValidateResult is not null) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                // ValidationProblemDetails <- errors[]
                Errors = accountValidateResult
            });

            // generate address model
            var address = reqObj.ToAddressModel();
            // validate address model
            var addressValidateResult = await address.ValidateAsync();
            if (addressValidateResult is not null) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                // ValidationProblemDetails <- errors[]
                Errors = addressValidateResult
            });
            // generate address id 
            var addressId = Guid.NewGuid().ToString();
            // set id to model
            address.AddressId = addressId;

            // generate member id 
            var memberId = Guid.NewGuid().ToString();
            // convert to member model
            var member = reqObj.ToMemberModel(_appSettings.DefaultAvatar, _appSettings.DateFormat);
            // validate member model
            var memberValidateResult = await member.ValidateAsync();
            if (memberValidateResult is not null) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                // ValidationProblemDetails <- errors[]
                Errors = memberValidateResult
            });
            // set id to model
            member.MemberId = memberId;
            member.Address = address;
            member.EmailNavigation = account;

            // add member
            member = await _memberService.CreateAsync(member);
            if (member is not null)
                // get member license type desc
                member.LicenseType = await _licenseTypes.GetAsync(Convert.ToInt32(member.LicenseTypeId));


            return new ObjectResult(new { Member = member }) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpGet]
        [Route("members/{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetMember([FromRoute] Guid id)
        {
            // get member by id
            var member = await _memberService.GetAsync(id);
            // not found nay member match id
            if (member is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any member match id"
            });

            // found member
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = member
            });
        }

        [HttpGet]
        [Route("members")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllMember()
        {
            var members = await _memberService.GetAllAsync();
            
            // not found any members
            if (members is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found any members"
            });

            // return members, totalPage, pageIndex
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Members = members
                }
            });
        }

        [HttpGet]
        [Route("members/{page:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllMember([FromRoute] int page = 1)
        {
            // memory caching
            if (!_memoryCache.TryGetValue(_appSettings.MembersCacheKey, out IEnumerable<MemberModel> members))
            {
                // get all members
                members = await _memberService.GetAllAsync();
                // cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // none access exceeds 45s <- remove cache
                    .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                    // after 10m from first access <- remove cache
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(600))
                    // cache priority
                    .SetPriority(CacheItemPriority.Normal);
                // set cache
                _memoryCache.Set(_appSettings.MembersCacheKey, members, cacheEntryOptions);
            }
            else
            {
                members = (IEnumerable<MemberModel>)_memoryCache.Get(_appSettings.MembersCacheKey);
            }

            // page size 
            int pageSize = _appSettings.PageSize;
            // paging
            var list = PaginatedList<MemberModel>.CreateByIEnumerable(members, page, pageSize);

            // not found any members
            if (members is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found any members"
            });

            // return members, totalPage, pageIndex
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Members = list,
                    TotalPage = list.TotalPage,
                    PageIndex = list.PageIndex
                }
            });
        }

        [HttpGet]
        [Route("members/{page:int}/filters")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllMemberByFilter([FromQuery] MemberFilter filters, [FromRoute] int page = 1)
        {
            // get all members
            var members = await _memberService.GetAllByFilterAsync(filters);
            // page size
            int pageSize = _appSettings.PageSize;
            // paging
            var result = PaginatedList<MemberModel>.CreateByIEnumerable(members, page, pageSize);

            // not found any members
            if (result.Count == 0) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found any members"
            });

            // return members, totalPage, pageIndex
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Members = result,
                    TotalPage = result.TotalPage,
                    PageIndex = result.PageIndex
                }
            });
        }

        [HttpDelete]
        [Route("members/{id:Guid}/delete")]
        public async Task<IActionResult> DeleteMember([FromRoute] Guid id)
        {
            // get member
            var member = await _memberService.GetAsync(id);
            // not exist
            if (member is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any members match id {id}"
            });
            // delete member
            await _memberService.DeleteAsync(Guid.Parse(member.MemberId));
            // delete account
            await _accountService.DeleteAsync(member.Email);
            // delete address
            await _addressService.DeleteAsync(Guid.Parse(member.AddressId));

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = $"Delete member {id} successfully"
            });
        }

        [HttpGet]
        [Route("members/{id:Guid}/update")]
        [Authorize]
        public async Task<IActionResult> UpdateMember([FromRoute] Guid id)
        {
            // get member by id
            var member = await _memberService.GetAsync(id);
            // get all license types
            var licenseTypes = await _licenseTypes.GetAllAsync();
            // not found any member match id
            if (member is null) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any member with id {id}"
            });

            // return data response <- found member
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Member = member,
                    LicenseTypes = licenseTypes
                }
            });
        }

        [HttpPut]
        [Route("members/{id:Guid}/update")]
        [Authorize]
        public async Task<IActionResult> UpdateMember([FromRoute] Guid id, [FromBody] MemberUpdateRequest reqObj)
        {
            // convert to member model <- extension method
            var member = reqObj.ToMemberModel(_appSettings.DateFormat);
            // validator
            var errors = await member.ValidateAsync();
            if (errors is not null) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                // ValidationProblemDetails <- errors[]
                Errors = errors
            });
            // update status
            var isSucess = await _memberService.UpdateAsync(id, member);

            // update failed <- not found 
            if (!isSucess) return NotFound(new BaseResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not foud any member match id {id}"
            });

            // update success
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = $"Update member {id} sucessfully"
            });
        }

        [HttpPut]
        [Route("members/{id:Guid}/hide")]
        public async Task<IActionResult> HideMember([FromRoute] Guid id) 
        {
            // get member by id
            var member = await _memberService.GetAsync(id);
            // not found
            if(member is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any members match id {id}"
                });
            }

            // hide member <- change active status
            var isSucess = await _memberService.HideMemberAsync(id);

            if (!isSucess) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = $"Hide member id {id} succesfully"
            });
        }
        
        [HttpGet]
        [Route("members/export-excel")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ExportToExcel([FromQuery] MemberFilter filters)
        {
            // Get courses by filters
            var members = await _memberService.GetAllByFilterAsync(filters);
            if (members is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Not found any members to export excel"
                });

            }
            
            // Start to exporting excel
            // Create stream contain file
            var stream = new MemoryStream();

            using (var xlPackgage = new ExcelPackage(stream))
            {
                // Define a worksheet
                var worksheet = xlPackgage.Workbook.Worksheets.Add("Members");

                // First Row
                var startRow = 3;

                // Worksheet details
                worksheet.Cells["A1"].Value = "List of Members";
                using (var r = worksheet.Cells["A1:C1"])
                {
                    // Merge next 2 col
                    r.Merge = true;
                }

                // Table header
                worksheet.Cells["A2"].Value = "Id";
                worksheet.Cells["B2"].Value = "First Name";
                worksheet.Cells["C2"].Value = "Last Name";
                worksheet.Cells["D2"].Value = "Date Birth";
                worksheet.Cells["E2"].Value = "Phone";
                worksheet.Cells["F2"].Value = "Email";
                worksheet.Cells["G2"].Value = "Street";
                worksheet.Cells["H2"].Value = "District";
                worksheet.Cells["I2"].Value = "City";
                worksheet.Cells["J2"].Value = "License Type";
                // Table rows
                var row = startRow;
                foreach (var m in members)
                {
                    // Get member address
                    m.Address = await _addressService.GetAsync(Guid.Parse(m.AddressId));
                    m.LicenseType = await _licenseTypeService.GetAsync(Convert.ToInt32(m.LicenseTypeId));
                    // set row record
                    worksheet.Cells[row, 1].Value = m.MemberId.ToString();
                    worksheet.Cells[row, 2].Value = m.FirstName;
                    worksheet.Cells[row, 3].Value = m.LastName;
                    worksheet.Cells[row, 4].Value = m.DateBirth.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 5].Value = m.Phone;
                    worksheet.Cells[row, 6].Value = m.Email;
                    worksheet.Cells[row, 7].Value = m.Address.Street;
                    worksheet.Cells[row, 8].Value = m.Address.District;
                    worksheet.Cells[row, 9].Value = m.Address.City;
                    worksheet.Cells[row, 10].Value = m.LicenseType.LicenseTypeDesc;

                    // next row
                    ++row;
                }

                xlPackgage.Workbook.Properties.Title = "Members List";
                xlPackgage.Workbook.Properties.Author = "Admin  ";
                await xlPackgage.SaveAsync();
            }
            // read from position
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "members.xlsx");
        }

        [HttpPost]
        [Route("members/import-excel")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ImportToExcel(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file?.Length > 0)
                {
                    // Starting import to excel
                    // Create file stream
                    var stream = file.OpenReadStream();

                    // Create excel package
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // Get first worksheet from package
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        // Get worksheet total rows
                        var rowCount = worksheet.Dimension.Rows;

                        // init members list
                        var members = new List<MemberModel>();

                        // Validation all excel sheets
                        for (var row = 2; row <= rowCount; ++row)
                        {
                            // Row data
                            var firstName = worksheet.Cells[row, 1].Value?.ToString();
                            var lastName = worksheet.Cells[row, 2].Value?.ToString();
                            var dateBirth = DateTime.ParseExact(worksheet.Cells[row, 3].Value?.ToString(),
                                "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                            var phone = worksheet.Cells[row, 4].Value?.ToString();
                            var street = worksheet.Cells[row, 5].Value?.ToString();
                            var district = worksheet.Cells[row, 6].Value?.ToString();
                            var city = worksheet.Cells[row, 7].Value?.ToString();
                            var licenseTypeDesc = worksheet.Cells[row, 8].Value?.ToString();
                            var email = worksheet.Cells[row, 9].Value?.ToString();
                            var password = worksheet.Cells[row, 10].Value?.ToString();

                            // Add member account
                            // Get member role 
                            var memberRole = await _roleService.GetMemberRoleIdAsync();
                            // Generate account model
                            var account = new AccountModel
                            {
                                Email = email,
                                Password = password,
                                RoleId = memberRole.RoleId,
                                IsActive = true
                            };
                            // Validate account model
                            var accountValidateResult = await account.ValidateAsync();
                            if (accountValidateResult is not null) return BadRequest(new ErrorResponse
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = $"Cause error at member {firstName} {lastName}, row {row}",
                                // ValidationProblemDetails <- errors[]
                                Errors = accountValidateResult
                            });
                            // encoding password
                            account.Password = PasswordHelper.ConvertToEncrypt(password);

                            // Add member address
                            var addressId = Guid.NewGuid().ToString();
                            // Generate address model
                            var address = new AddressModel
                            {
                                AddressId = addressId,
                                Street = street,
                                District = district,
                                City = city
                            };
                            // Validate address model
                            var addressValidateResult = await address.ValidateAsync();
                            if (addressValidateResult is not null) return BadRequest(new ErrorResponse
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = $"Cause error at member {firstName} {lastName}, row {row}",
                                // ValidationProblemDetails <- errors[]
                                Errors = addressValidateResult
                            });

                            // Add to members list
                            // Get license type id
                            var licenseType = await _licenseTypeService.GetByDescAsync(licenseTypeDesc.ToUpper());
                            var memberId = Guid.NewGuid().ToString();
                            // Generate member model 
                            var member = new MemberModel
                            {
                                MemberId = memberId,
                                FirstName = firstName,
                                LastName = lastName,
                                DateBirth = dateBirth,
                                Phone = phone,
                                Email = email,
                                Address = address,
                                LicenseTypeId = licenseType.LicenseTypeId,
                                EmailNavigation = account
                            };
                            // Validate member model
                            var memberValidateResult = await member.ValidateAsync();
                            if (memberValidateResult is not null) return BadRequest(new ErrorResponse
                            {
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = $"Cause error at member {firstName} {lastName}, row {row}",
                                // ValidationProblemDetails <- errors[]
                                Errors = memberValidateResult
                            });

                            // Add member to list
                            members.Add(member);
                        }

                        // Add range members
                        var createdMembers = await _memberService.CreateRangeAsync(members);
                        // Get total members
                        var totalMembers = createdMembers.Count();

                        // clear cache
                        if (_memoryCache.Get(_appSettings.MembersCacheKey) is not null)
                            _memoryCache.Remove(_appSettings.MembersCacheKey);

                        // Import Sucessfully
                        if (totalMembers > 0) return Ok(new BaseResponse
                        {
                            StatusCode = StatusCodes.Status200OK,
                            Message = $"Import excel file successfully, total {totalMembers} members created",
                            Data = createdMembers
                        });
                    }
                }
            }
            // Import Failed
            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Import excel file failed."
            });
        }

        // License Register Form
        [HttpGet]
        [Route("members/license-form")]
        public async Task<IActionResult> LicenseFormRegister()
        {
            var licenseTypes = await _licenseTypes.GetAllAsync();

            if (licenseTypes is null) return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Something went wrong"
            });

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = licenseTypes
            });
        }

        [HttpPost]
        [Route("members/license-form")]
        public async Task<IActionResult> LicenseFormRegister([FromForm] LicenseRegisterFormRequest reqObj)
        {
            // validator 
            var validator = new Validation.LicenseRegisterFormValidator();
            // validate fields
            var result = await validator.ValidateAsync(reqObj);
            if (!result.IsValid) // cause errors
            {
                // generate ValidationProblemDetails and return error resp
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = result.ToProblemDetails()
                });
            }

            // check already exist
            var existLicenseForm = _licenseRegisterFormService.GetByMemberId(reqObj.MemberId);
            if(existLicenseForm is not null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"License register form are already exist. Please delete to create new or update"
                });
            }

            // generate image id
            var imageId = Guid.NewGuid();
            // upload image
            //await _imageService.UploadImageAsync(imageId, reqObj.Image);

            // generate identity image id
            var identityImageId = Guid.NewGuid();
            // upload image
            //await _imageService.UploadImageAsync(identityImageId, reqObj.IdentityImage);

            // generate health certification image id
            // upload image
            var healthCerImageId = Guid.NewGuid();
            //await _imageService.UploadImageAsync(healthCerImageId, reqObj.HealthCertificationImage);

            // generate license form register
            var licenseRegisterFormModel = reqObj.ToLicenseFormRegisterModel();
            // set images id
            licenseRegisterFormModel.Image = imageId.ToString();
            licenseRegisterFormModel.IdentityCardImage = identityImageId.ToString();
            licenseRegisterFormModel.HealthCertificationImage = healthCerImageId.ToString();


            // create license form register
            var lfRegister = await _memberService.CreateLicenseRegisterFormAsync(licenseRegisterFormModel, reqObj.MemberId);

            if (lfRegister is null) return StatusCode(
                StatusCodes.Status500InternalServerError, "Something went wrong");

            return new ObjectResult(new { LicenseRegisterForm = lfRegister }) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut]
        [Route("members/license-form/{id:int}/update")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateLicenseFormRegister([FromRoute] int id,
            [FromForm] LicenseRegisterFormUpdate reqObj) 
        {
            // get license register form by id
            var lfRegister = await _licenseRegisterFormService.GetAsync(id);
            // 404 Not Found <- not found any id match
            if (lfRegister is null) return NotFound(new BaseResponse { 
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any license register form match id {id}"
            });
            else if(lfRegister.RegisterFormStatusId == 2)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "License form status are already approve, not able to update." +
                    " Please remove and create again"
                });
            }

            // update license register form
            // update privacy image
            if(reqObj.Image is not null)
            {
                // remove prev image
                //await _imageService.DeleteImageAsync(Guid.Parse(lfRegister.Image));
                // upload new image to clound
                //await _imageService.UploadImageAsync(Guid.Parse(lfRegister.Image),
                //    reqObj.Image);
            }
            // update identity image
            if (reqObj.IdentityImage is not null)
            {
                // remove prev image
                //await _imageService.DeleteImageAsync(Guid.Parse(lfRegister.IdentityCardImage));
                // upload new image to clound
                //await _imageService.UploadImageAsync(Guid.Parse(lfRegister.IdentityCardImage),
                    //reqObj.IdentityImage);
            }
            // update health certification image
            if (reqObj.HealthCertificationImage is not null)
            {
                // remove prev image
                //await _imageService.DeleteImageAsync(Guid.Parse(lfRegister.HealthCertificationImage));
                // upload new image to clound
                //await _imageService.UploadImageAsync(Guid.Parse(lfRegister.HealthCertificationImage),
                    //reqObj.HealthCertificationImage);
            }

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = $"Update license register form id {id} succesfully"
            });
        }

        [HttpPut]
        [Route("members/license-form/{id:int}/approve")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> LicenseFormRegisterApproval([FromRoute] int id) 
        {
            // get license form by member id
            var lfRegister = await _memberService.GetByLicenseRegisterFormIdAsync(id);
            // not found
            if (lfRegister is null) 
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any license form of member id {id}"
                });
            }

            // approve member license form register
            var isSucess = await _licenseRegisterFormService.ApproveAsync(id);

            if (isSucess) 
            {
                return Ok(new BaseResponse { 
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"License form register approved successfully"
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [Route("members/feedback/mentor")]
        public async Task<IActionResult> FeedbackMentor([FromForm] FeedbackMentorRequest reqObj) 
        {
            // generate feedback model
            var feedback = reqObj.ToFeedbackModel();

            // check exist member, mentor
            var member = await _memberService.GetAsync(Guid.Parse(feedback.MemberId));
            var mentor = await _staffService.GetMentorAsync(Guid.Parse(feedback.StaffId));
            if(member is null)
            {
                return NotFound(new BaseResponse { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any member id {reqObj.MemberId}"
                });
            }
            if(mentor is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any mentor id {reqObj.MentorId}"
                });
            }

            // create feedback
            var isSucess = await _feedbackService.CreateAsync(feedback);

            if (isSucess) {
                return Ok(new BaseResponse { 
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Feedback mentor successfully"
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [Route("members/feedback/course")]
        public async Task<IActionResult> FeedbackCourse([FromForm] FeedbackCourseRequest reqObj) 
        {
            // generate feedback model
            var feedback = reqObj.ToFeedbackModel();

            // check exist member, mentor
            var member = await _memberService.GetAsync(Guid.Parse(feedback.MemberId));
            var course = await _courseService.GetAsync(Guid.Parse(feedback.CourseId));
            if (member is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any member match id {reqObj.MemberId}"
                });
            }
            if (course is null)
            {
                return NotFound(new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Not found any course match id {reqObj.CourseId}"
                });
            }

            // create feedback
            var isSucess = await _feedbackService.CreateAsync(feedback);

            if (isSucess)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Feedback course successfully"
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        [Route("members/{id:Guid}/schedule")]
        public async Task<IActionResult> GetMemberCalendar([FromRoute] Guid id)
        {
            // get course reservation
            var courseReservation = await _courseReservationService.GetByMemberAsync(id);
            if (courseReservation is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any learning schedule"
                });
            }
            // get course by id 
            var course = await _courseService.GetAsync(Guid.Parse(courseReservation.CoursePackage.CourseId));
            // set null mentors list 
            course.Mentors = null;
            course.FeedBacks = null;

            // get staff by id
            var staff = await _staffService.GetAsync(Guid.Parse(courseReservation.StaffId));
            staff.Courses = null;
            staff.SelfDescription = string.Empty;
            staff.FeedBacks = null;
            staff.EmailNavigation = null;

            // generate current date time 
            var currDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);

            // check current date time with course start date
            var courseStartDate = DateTime.ParseExact(Convert.ToDateTime(course.StartDate).ToString("yyyy-MM-dd"),
                _appSettings.DateFormat, CultureInfo.InvariantCulture);
            var courseTotalMonth = Convert.ToInt32(course.TotalMonth);
            if (currDate < courseStartDate &&
               currDate > courseStartDate.AddMonths(courseTotalMonth))
            {
                currDate = courseStartDate;
            }

            // get calendar by current date
            var weekday = await _weekDayScheduleService.GetByDateAndCourseId(currDate, 
                Guid.Parse(courseReservation.CoursePackage.CourseId));
            if(weekday is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Not found any learning schedule"
                });
            }
            // get all weekday of calendar
            var weekdays = await _weekDayScheduleService.GetAllByCourseId(
                Guid.Parse(courseReservation.CoursePackage.CourseId));
            // get all slots 
            var slots = await _slotService.GetAllAsync();
            // convert to list of course
            var listOfSlotSchedule = slots.ToList();

            // get learning schedule for each slot
            foreach (var s in slots)
            {
                var teachingSchedules
                    = await _teachingScheduleService.GetBySlotAndWeekDayScheduleAsync(s.SlotId,
                        weekday.WeekdayScheduleId,
                        Guid.Parse(courseReservation.StaffId));
                s.TeachingSchedules = teachingSchedules.ToList();
            }

            // response
            var response = new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Course = course,
                    Mentor = staff,
                    Filter = weekdays.Select(x => new
                    {
                        Id = x.WeekdayScheduleId,
                        Desc = x.WeekdayScheduleDesc
                    }),
                    Weekdays = weekday,
                    SlotSchedules = listOfSlotSchedule
                }
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("members/{id:Guid}/schedule/filter")]
        public async Task<IActionResult> GetMemberCalendarByFilter([FromRoute] Guid id,[FromQuery] LearningScheduleFilter filters)
        {
            // get course reservation
            var packageReservation = await _courseReservationService.GetByMemberAsync(id);
            if(packageReservation is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any learning schedule"
                });
            }

            WeekdayScheduleModel weekday = null!;
            // get weekday by id
            if (filters.WeekDayScheduleId is not null)
            {
                weekday = await _weekDayScheduleService.GetAsync(
                    Convert.ToInt32(filters.WeekDayScheduleId));
            }

            if (filters.LearningDate is not null)
            {
                // get learning date by filters
                var learningDate = await _teachingScheduleService.GetMemberScheduleByFilterAsync(filters, id);

                if (learningDate is null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = $"Not found any schedule match date {filters.LearningDate}"
                    });
                }

                // get calendar by id
                weekday = await _weekDayScheduleService.GetAsync(
                    Convert.ToInt32(learningDate.WeekdayScheduleId));
            }

            // get all weekday of calendar
            var weekdays = await _weekDayScheduleService.GetAllByCourseId(
                Guid.Parse(packageReservation.CoursePackage.CourseId));
            // get all slots 
            var slots = await _slotService.GetAllAsync();
            // convert to list of course
            var listOfSlotSchedule = slots.ToList();
            // get teaching schedule for each slot
            foreach (var s in slots)
            {
                /*
                var teachingSchedules
                    = await _teachingScheduleService.GetBySlotAndWeekDayScheduleOfMemberAsync(s.SlotId,
                        weekday.WeekdayScheduleId,
                        Guid.Parse(courseReservation.StaffId), id);
                s.TeachingSchedules = teachingSchedules.ToList();*/
                var teachingSchedules
                    = await _teachingScheduleService.GetBySlotAndWeekDayScheduleAsync(s.SlotId,
                        weekday.WeekdayScheduleId,
                        Guid.Parse(packageReservation.StaffId));
                s.TeachingSchedules = teachingSchedules.ToList();
            }
            // get course by id 
            var course = await _courseService.GetAsync(Guid.Parse(weekday.CourseId));
            course.Mentors = null;
            course.FeedBacks = null;
            course.Curricula = null;
            // get staff by id
            var staff = await _staffService.GetAsync(Guid.Parse(packageReservation.StaffId));
            staff.Courses = null;
            staff.SelfDescription = string.Empty;
            staff.FeedBacks = null;
            staff.EmailNavigation = null;

            // response
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Course = course,
                    Mentor = staff,
                    Filter = weekdays.Select(x => new {
                        Id = x.WeekdayScheduleId,
                        Desc = x.WeekdayScheduleDesc
                    }),
                    Weekdays = weekday,
                    SlotSchedules = listOfSlotSchedule
                }
            });
        }

        [HttpPost]
        [Route("members/schedule")]
        public async Task<IActionResult> LearningScheduleRegister([FromBody] LearningScheduleRequest reqObj)
        {
            // check exist in course reservation
            var courseReservation = await _courseReservationService.GetByMemberAsync(reqObj.MemberId);
            if (courseReservation is null)
            {
                return BadRequest(new BaseResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any learning course package"
                });
            }

            // check course reservation status
            if(courseReservation.ReservationStatusId == 1)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Vui lòng thanh toán khóa học để được xếp lớp"
                });
            }

            // get all member rollcallbook
            var rcbooks = await _rollCallBookService.GetAllByMemberIdAsync(
                Guid.Parse(courseReservation.MemberId));

            // get course by id 
            var course = await _courseService.GetAsync(
                Guid.Parse(courseReservation.CoursePackage.CourseId));

            // get course package by id
            var coursePackage = await _courseService.GetPackageAsync(
                Guid.Parse(courseReservation.CoursePackageId));

            // count member total rollcallbook
            var rcbCount = 0;
            if (rcbooks is not null)
            {
                rcbCount = rcbooks.Where(x => x.IsAbsence == false).Count();
            }

            // total registered session > course total session <- not allow to register 
            var totalRegisteredSession = rcbooks is not null ? rcbooks?.Count() : 0;
            var isOverTotalSession = (totalRegisteredSession) > coursePackage.TotalSession ? true : false;
            if (isOverTotalSession)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not allow to register more session, " +
                    $"because member total session is over total course session",
                    Data = new
                    {
                        TotalRegisteredSession = totalRegisteredSession
                    }
                });
            }

            // get teaching schedule
            var teachingSchedule = await _teachingScheduleService.GetAsync(reqObj.TeachingScheduleId);
            // check exist teaching schedule
            if (teachingSchedule is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any schedule match required"
                });
            }
            


            // get weekday schedule id by teaching date request
            var weekday = await _weekDayScheduleService.GetByDateAndCourseId(teachingSchedule.TeachingDate
                , Guid.Parse(course.CourseId));
            // check teaching date exist
            if (weekday is null)
            {
                var date = Convert.ToDateTime(course.StartDate);
                var totalMonth = Convert.ToInt32(course.TotalMonth);
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any course date match {teachingSchedule.TeachingDate.ToString("dd/MM/yyyy")} " +
                    $"course date from {date} " +
                    $"- {date.AddMonths(totalMonth)}"
                });
            }

            // generate rollcallbook model
            var rcbModel = reqObj.ToRollCallBookModel();
            rcbModel.MemberTotalSession = rcbCount;

            var registeredSchedule = await _teachingScheduleService.GetByMentorIdAndTeachingDateAsync(
                Convert.ToInt32(teachingSchedule.WeekdayScheduleId),
                Guid.Parse(courseReservation.StaffId),
                teachingSchedule.TeachingDate,
                Convert.ToInt32(teachingSchedule.SlotId));

            // check teaching schedule already register
            if (registeredSchedule is not null)
            {
                // convert to list obj
                var schedules = registeredSchedule.RollCallBooks.ToList();
                if(registeredSchedule.RollCallBooks.Count > 0)
                {
                    return BadRequest(new BaseResponse { 
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = $"Date {registeredSchedule.TeachingDate.ToString("dd/MM/yyyy")} is already register " +
                        $"by {schedules[0].Member.FirstName} {schedules[0].Member.LastName}"
                    });
                }
            }

            // check member in course
            if(courseReservation.StaffId != teachingSchedule.StaffId)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Member {reqObj.MemberId} not allow to register this " +
                        $"schedule"
                });
            }

            // check exist member
            var member = await _memberService.GetAsync(reqObj.MemberId);
            if(member is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Not found any member match id {reqObj.MemberId}"
                });
            }

            // add member rcb to teaching schedule
            var isSuccess = await _teachingScheduleService.AddRollCallBookAsync(
                teachingSchedule.TeachingScheduleId, rcbModel);

            /// TODO: Get Created Schedule 
            if (isSuccess)
            {
                // add course package to teaching schedule id
                await _teachingScheduleService.AddCoursePackageAsync(teachingSchedule.TeachingScheduleId,
                    Guid.Parse(courseReservation.CoursePackageId));

                //// add teaching schedule vehicle
                //await _teachingScheduleService.AddVehicleAsync(
                //    teachingSchedule.TeachingScheduleId, 
                //    Convert.ToInt32(courseReservation.VehicleId));

                return Ok(new BaseResponse { 
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Register schedule successfully"
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}