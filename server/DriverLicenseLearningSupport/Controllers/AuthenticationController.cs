using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Models;
using AutoMapper.Execution;
using DriverLicenseLearningSupport.Services;
using System.Net;
using System.IO;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Caching.Memory;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // DateTime format
        //public static string dateFormat = "yyyy-MM-dd";
        // Default avatar
        //public static string defaultAvatar = "a42e811d-d22b-4ede-b955-1437ebaeeb9d";
        // cache key
        //private readonly static string _memberCacheKey = "MembersCacheKey";
        // Dependency Injection Obj
        private readonly IAccountService _accountService;
        private readonly IMemberService _memberService;
        private readonly IStaffService _staffService;
        private readonly IEmailService _emailService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IJobTitleService _jobTitleService;
        private readonly IRoleService _roleService;
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;

        public AuthenticationController(IAccountService accountService,
            IMemberService memberService,
            IEmailService emailService,
            IStaffService staffService,
            ILicenseTypeService licenseTypeService,
            IJobTitleService jobTitleService,
            IRoleService roleService,
            IMemoryCache cache,
            IOptionsMonitor<AppSettings> monitor)
        {
            _accountService = accountService;
            _memberService = memberService;
            _staffService = staffService;
            _emailService = emailService;
            _licenseTypeService = licenseTypeService;
            _jobTitleService = jobTitleService;
            _roleService = roleService;
            _cache = cache;
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
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = result.ToProblemDetails()
                });
            }

            // check login
            var account = await _accountService.CheckLoginAsync(reqObj.Username,
                PasswordHelper.ConvertToEncrypt(reqObj.Password));

            if (account == null) // account not exists
                // return Unauthorized 
                return Unauthorized(new BaseResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Wrong username or password!",
                });

            //get account info by role
            Object accountInfo;
            if (account.Role.Name.Equals("Member"))
            {
                // get member info
                accountInfo = await _memberService.GetByEmailAsync(account.Email);
            }
            else
            {
                // get staff (mentor/staff/admin) info
                accountInfo = await _staffService.GetByEmailAsync(account.Email);
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
            var licenseTypes = await _licenseTypeService.GetAllAsync();
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = licenseTypes
            });
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest reqObj)
        {
            // check exist account
            var accountExist = await _accountService.GetByEmailAsync(reqObj.Username);
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
            // validate account model
            var accountValidateResult = await account.ValidateAsync();
            if (accountValidateResult is not null) return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                // ValidationProblemDetails <- errors[]
                Errors = accountValidateResult
            });

            // generate address
            var address = reqObj.ToAddressModel();
            var addressId = Guid.NewGuid().ToString();
            address.AddressId = addressId;

            // generate member
            var member = reqObj.ToMemberModel(_appSettings.DateFormat);
            var memberId = Guid.NewGuid().ToString();
            member.MemberId = memberId;
            member.EmailNavigation = account;
            member.Address = address;
            member.IsActive = true;
            member.AvatarImage = _appSettings.DefaultAvatar;

            // create member
            await _memberService.CreateAsync(member);

            // find created member
            member = await _memberService.GetAsync(Guid.Parse(memberId));

            // clear get all members cache
            if (_cache.Get(_appSettings.MembersCacheKey) is not null) 
            {
                // remove memory cache
                _cache.Remove(_appSettings.MembersCacheKey);
            }

            return Ok(new BaseResponse
            {
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

        [HttpPost("authentication/staff")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> StaffRegister([FromBody] StaffRegisterRequest reqObj) 
        {
            // check exist account
            var accountExist = await _accountService.GetByEmailAsync(reqObj.Email);
            if (accountExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        Message = "Email already exist!"
                    });
            }

            // generate account model
            var account = reqObj.ToAccountModel();
            // validation
            var result = await account.ValidateAsync();
            if(result is not null) 
            {
                return BadRequest(new ErrorResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = result
                });
            }

            // create staff account
            var isSucess = await _accountService.CreateAsync(account);

            if (!isSucess) // cause error
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = "Create staff account succesfully",
            });
        }

        [HttpPost("authentication/forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required][EmailAddress] string email)
        {
            var account = await _accountService.GetByEmailAsync(email);
            if (account != null)
            {
                var passwordResetToken = (new JwtHelper(_appSettings)).GeneratePasswordResetToken(email);
                // Url + Action + Controller 
                //var forgotPasswordLink = Url.Action("ResetPassword", "Authentication", new { passwordResetToken, email = account.Email }, Request.Scheme);
                var forgotPasswordLink = Url.Action("ResetPassword", "Authentication",
                    values: new { passwordResetToken, email = account.Email }, Request.Scheme, host: "localhost:3000");
                var message = new EmailMessage(new string[] { account.Email! }, "Forgot Password Link", forgotPasswordLink!);
                _emailService.SendEmail(message);

                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Password change request has been sent to Email {account.Email}. Please see email and access link"
                });
            }
            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Unable to send email, please try again!"
            });
        }

        [HttpGet("authentication/reset-password")]
        public async Task<IActionResult> ResetPassword(string passwordResetToken, string email)
        {
            return Ok(new { passwordResetToken, email });
        }

        [HttpPost("authentication/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            var account = await _accountService.GetByEmailAsync(resetPassword.Email);
            if (account != null)
            {
                var newPasswordEncrypt = PasswordHelper.ConvertToEncrypt(resetPassword.Password);
                await _accountService.ResetPasswordAsync(resetPassword.Email, newPasswordEncrypt);

                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Password changed successfully"
                });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new BaseResponse { Message = "Password change failed." });
        }

        [HttpGet]
        [Route("authentication/logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok(new BaseResponse { 
                StatusCode=StatusCodes.Status200OK,
                Message = "Logout succesfully"
            });
        }
    }
}