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

namespace DriverLicenseLearningSupport.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // DateTime format
        public static string dateFormat = "yyyy-MM-dd";
        // Dependency Injection Obj
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly IMemberService _memberService;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;

        public AuthenticationController(IAccountService accountService,
            IAddressService addressService,
            IMemberService memberService,
            IEmailService emailService,
            IOptionsMonitor<AppSettings> monitor)
        {
            _accountService = accountService;
            _addressService = addressService;
            _memberService = memberService;
            _emailService = emailService;
            _appSettings = monitor.CurrentValue;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest reqObj)
        {
            // init validator
            LoginValidator validator = new LoginValidator();
            // get ValidationResult from validator
            var result = await validator.ValidateAsync(reqObj);
            if (!result.IsValid) // cause validation errors
            {
                // return BadRequest with ValidationProblemDetails
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = result.ToProblemDetails()
                });
            }

            // check login
            var account = await _accountService.CheckLoginAsync(reqObj.username,
                PasswordHelper.ConvertToEncrypt(reqObj.password));

            if (account == null) // account not exists
                // return Unauthorized 
                return Unauthorized(new BaseResponse { 
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Wrong username or password!",
                });

            // generate jwt bearer token
            var token = (new JwtHelper(_appSettings)).GenerateToken(account);

            // return Ok with Token
            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Login Sucess",
                Data = token
            });
        }

        [HttpPost]
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
                        Message = "User already exists!"
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
            member.MemberId = Guid.NewGuid().ToString();
            member.Email = reqObj.Username;
            member.AddressId = addressId;
            member.IsActive = true;
            await _memberService.CreateAsync(member);

            return Ok(new BaseResponse {
                StatusCode = StatusCodes.Status200OK,
                Message = "Sign Up Sucess"
            });
        }

        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string email)
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
                    Message = $"Password changed request is sent on Email {account.Email}. Please Open your email & click the link"});
            }
            return BadRequest(new BaseResponse { 
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Could not send link to email, please try again!"
            });
        }

        [HttpGet]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(string passwordResetToken, string email) 
        {
            return Ok(new { passwordResetToken, email });
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword) 
        {
            var account = await _accountService.FindByEmailAsync(resetPassword.Email);
            if(account != null) 
            {
                var newPasswordEncrypt = PasswordHelper.ConvertToEncrypt(resetPassword.Password);
                await _accountService.ResetPasswordAsync(resetPassword.Email, newPasswordEncrypt);

                return Ok(new BaseResponse {
                    StatusCode = StatusCodes.Status200OK, Message = "Password change successfully" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new BaseResponse { Message = "Password change failed." });
        }
    }
}
