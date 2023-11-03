using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Utils;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class AccountAddRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int RoleId { get; set; }
    }

    public static class AccountAddRequestExtension
    {
        public static AccountModel ToAccountModel(this AccountAddRequest reqObj)
        {
            return new AccountModel
            {
                Email = reqObj.Email,
                Password = PasswordHelper.ConvertToEncrypt(reqObj.Password),
                RoleId = reqObj.RoleId,
                IsActive = true
            };
        }
    }
}
