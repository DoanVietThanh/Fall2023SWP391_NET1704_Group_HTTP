using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using DriverLicenseLearningSupport.Services.impl;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Entities;
using AutoMapper.Execution;
using DriverLicenseLearningSupport.Services;
using System.Net;
using System.IO;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // DateTime format
        public static string dateFormat = "yyyy-MM-dd";
        // Default avatar
        public static string defaultAvatar = "1e4486a9-4f8e-45c1-b021-9be582701e3d";
        // Dependency Injection Obj
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly IMemberService _memberService;
        private readonly IStaffService _staffService;
        private readonly IEmailService _emailService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IJobTitleService _jobTitleService;
        private readonly IRoleService _roleService;
        private readonly AppSettings _appSettings;

        public AuthenticationController(IAccountService accountService,
            IAddressService addressService,
            IMemberService memberService,
            IEmailService emailService,
            IStaffService staffService,
            ILicenseTypeService licenseTypeService,
            IJobTitleService jobTitleService,
            IRoleService roleService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _accountService = accountService;
            _addressService = addressService;
            _memberService = memberService;
            _staffService = staffService;
            _emailService = emailService;
            _licenseTypeService = licenseTypeService;
            _jobTitleService = jobTitleService;
            _roleService = roleService;
            _appSettings = monitor.CurrentValue;
        }

        [HttpPost("authentication/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest reqObj)
        {
            // init validator
            LoginValidator validator = new LoginValidator();
            // get ValidationResult from validator
            var result = await validator.ValidateAsync(reqObj);
            if (!result.IsValid) // cause validation errors
            {
                // return BadRequest with ValidationProblemDetails
                return BadRequest(new ErrorResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = result.ToProblemDetails()
                });
            }

            // check login
            var account = await _accountService.CheckLoginAsync(reqObj.Username,
                PasswordHelper.ConvertToEncrypt(reqObj.Password));

            if (account == null) // account not exists
                // return Unauthorized 
                return Unauthorized(new BaseResponse { 
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Wrong username or password!",
                });

            // get account info by role
            Object accountInfo;
            if (account.Role.Name.Equals("Member")){
                // get member info
                accountInfo = await _memberService.FindByEmailAsync(account.Email);
            } 
            else 
            {
                // get staff (mentor/staff/admin) info
                accountInfo = await _staffService.FindByEmailAsync(account.Email);
            }

            // generate jwt bearer token
            var token = (new JwtHelper(_appSettings)).GenerateToken(account);

            // return Ok with Token
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Login Success",
                Data = new 
                {
                    Token = token,
                    AccountInfo = accountInfo
                }
            });
        }

        [HttpGet("authentication")]
        public async Task<IActionResult> Register()
        {
            // get all license type
            var licenseTypes = await _licenseTypeService.FindAllAsync();
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = licenseTypes
            });
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest reqObj)
        {
            // validation
            RegisterRequestValidator validator = new RegisterRequestValidator();
            var result = await validator.ValidateAsync(reqObj);
            if (!result.IsValid)
            {
                // return BadRequest with ValidationProblemDetails
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = result.ToProblemDetails()
                });
            }

            // check exist account
            var accountExist = await _accountService.FindByEmailAsync(reqObj.Username);
            if (accountExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        Message = "Email already exist!"
                    });
            } 

            // generate account
            var account = reqObj.ToAccountModel();
            await _accountService.CreateAsync(account);

            // generate address
            var address = reqObj.ToAddressModel();
            var addressId = Guid.NewGuid().ToString();
            address.AddressId = addressId; 
            await _addressService.CreateAsync(address);

            // generate member
            var member = reqObj.ToMemberModel(dateFormat);
            var memberId = Guid.NewGuid().ToString();
            member.MemberId = memberId;
            member.Email = reqObj.Username;
            member.AddressId = addressId;
            member.IsActive = true;
            member.AvatarImage = defaultAvatar;
            await _memberService.CreateAsync(member);

            // find created member
            member = await _memberService.FindByIdAsync(Guid.Parse(memberId));

            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Message = "Register Success",
                Data = new 
                {
                    Account = account,
                    Address = address,
                    Member = member
                }
            });
        }

        [HttpGet]
        [Route("authentication/staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StaffRegister()
        {
            // get all license type
            var licenseTypes = await _licenseTypeService.FindAllAsync();
            // get all job title
            var jobTitles = await _jobTitleService.FindAllAsync();
            // get all account roles
            var roles = await _roleService.FindAllAsync();

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Data = new 
                {
                    LicenseTypes = licenseTypes,
                    JobTitles = jobTitles,
                    Roles = roles
                }
            });
        }

        [HttpPost]
        [Route("authentication/staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StaffRegister([FromBody] StaffRegisterRequest reqObj)
        {
            // validation
            StaffRegisterRequestValidator validator = new StaffRegisterRequestValidator();
            var result = await validator.ValidateAsync(reqObj);
            if (!result.IsValid)
            {
                // return BadRequest with ValidationProblemDetails
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = result.ToProblemDetails()
                });
            }

            // check account exist
            var accountExist = await _accountService.FindByEmailAsync(reqObj.Username);
            if(accountExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        Message = "Email already exist!"
                    });
            }

            //generate account
            var account = reqObj.ToAccountModel();
            await _accountService.CreateAsync(account);

            // generate address
            var address = reqObj.ToAddressModel();
            var addressId = Guid.NewGuid().ToString();
            address.AddressId = addressId;
            await _addressService.CreateAsync(address);

            // generate staff
            var staff = reqObj.ToStaffModel(dateFormat);
            var staffId = Guid.NewGuid().ToString();
            staff.StaffId = staffId;
            staff.Email = reqObj.Username;
            staff.AddressId = addressId;
            staff.IsActive = true;
            staff.AvatarImage = defaultAvatar;
            await _staffService.CreateAsync(staff);

            // find created staff by id
            staff = await _staffService.FindByIdAsync(Guid.Parse(staffId));

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = "Register Sucess",
                Data = new
                {
                    Staff = staff
                }
            });
        }

        [HttpPost]
        [Route("authentication/forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] [EmailAddress] string email)
        {
            var account = await _accountService.FindByEmailAsync(email);
            if (account != null)
            {
                var passwordResetToken = (new JwtHelper(_appSettings)).GeneratePasswordResetToken(email);
                // Url + Action + Controller 
                var forgotPasswordLink = Url.Action("ResetPassword", "Authentication", new { passwordResetToken, email = account.Email }, Request.Scheme);
                var message = new EmailMessage(new string[] { account.Email! }, "Forgot Password Link", forgotPasswordLink!);
                _emailService.SendEmail(message);
                
                return Ok(new BaseResponse {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Password change request has been sent to Email {account.Email}. Please see email and access link"
                });
            }
            return BadRequest(new BaseResponse { 
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Unable to send email, please try again!"
            });
        }

        [HttpGet]
        [Route("authentication/reset-password")]
        public async Task<IActionResult> ResetPassword(string passwordResetToken, string email) 
        {
            var token = PasswordHelper.ConvertToDecrypt(passwordResetToken);
            if(token != null) 
                return Ok(new { passwordResetToken, email });
            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Wrong token format!"
            });
        }

        [HttpPost]
        [Route("authentication/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword) 
        {
            var account = await _accountService.FindByEmailAsync(resetPassword.Email);
            if(account != null) 
            {
                var newPasswordEncrypt = PasswordHelper.ConvertToEncrypt(resetPassword.Password);
                await _accountService.ResetPasswordAsync(resetPassword.Email, newPasswordEncrypt);

                return Ok(new BaseResponse {
                    StatusCode = StatusCodes.Status200OK, Message = "Password changed successfully"
                });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new BaseResponse { Message = "Password change failed." });
        }
    }
}
