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
using DriverLicenseLearningSupport.Entities;
using AutoMapper.Execution;
using DriverLicenseLearningSupport.Services;
using System.Net;
using System.IO;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // DateTime format
        public static string dateFormat = "yyyy-MM-dd";
        // Default avatar
        public static string defaultAvatar = "a42e811d-d22b-4ede-b955-1437ebaeeb9d";
        // Dependency Injection Obj
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly IMemberService _memberService;
        private readonly IStaffService _staffService;
        private readonly IEmailService _emailService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly ILicenseRegisterFormService _licenseRegisterFormService;
        private readonly IJobTitleService _jobTitleService;
        private readonly IRoleService _roleService;
        private readonly AppSettings _appSettings;

        public AuthenticationController(IAccountService accountService,
            IAddressService addressService,
            IMemberService memberService,
            IEmailService emailService,
            IStaffService staffService,
            ILicenseTypeService licenseTypeService,
            ILicenseRegisterFormService licenseRegisterFormService,
            IJobTitleService jobTitleService,
            IRoleService roleService,
            IImageService imageService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _accountService = accountService;
            _addressService = addressService;
            _memberService = memberService;
            _staffService = staffService;
            _emailService = emailService;
            _licenseTypeService = licenseTypeService;
            _licenseRegisterFormService = licenseRegisterFormService;
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

            // get account info by role
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
            var member = reqObj.ToMemberModel(dateFormat);
            var memberId = Guid.NewGuid().ToString();
            member.MemberId = memberId;
            member.EmailNavigation = account;
            member.Address = address;
            member.IsActive = true;
            member.AvatarImage = defaultAvatar;

            // create member
            await _memberService.CreateAsync(member);

            // find created member
            member = await _memberService.GetAsync(Guid.Parse(memberId));

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

        [HttpGet]
        [Route("authentication/staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StaffRegister()
        {
            // get all license type
            var licenseTypes = await _licenseTypeService.GetAllAsync();
            // get all job title
            var jobTitles = await _jobTitleService.GetAllAsync();
            // get all account roles
            var roles = await _roleService.GetAllAsync();

            return Ok(new BaseResponse
            {
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
            // check account exist
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

            //generate account
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

            // generate staff
            var staff = reqObj.ToStaffModel(dateFormat);
            var staffId = Guid.NewGuid().ToString();
            staff.StaffId = staffId;
            staff.EmailNavigation = account;
            staff.Address = address;
            staff.IsActive = true;
            staff.AvatarImage = defaultAvatar;

            // create staff
            await _staffService.CreateAsync(staff);

            // find created staff by id
            staff = await _staffService.GetAsync(Guid.Parse(staffId));

            return Ok(new BaseResponse
            {
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

        [HttpGet]
        [Route("authentication/reset-password")]
        public async Task<IActionResult> ResetPassword(string passwordResetToken, string email)
        {
            return Ok(new { passwordResetToken, email });
        }

        [HttpPost]
        [Route("authentication/reset-password")]
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
    }
}