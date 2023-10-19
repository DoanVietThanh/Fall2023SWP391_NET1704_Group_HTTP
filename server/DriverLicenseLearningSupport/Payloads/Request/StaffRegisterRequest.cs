using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Utils;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class StaffRegisterRequest
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Password { get; set; }
        public int roleId { get; set; }
    }

    public static class StaffRegisterRequestExtension 
    {
        public static AccountModel ToAccountModel(this StaffRegisterRequest reqObj) 
        {
            return new AccountModel() 
            {
                Email = reqObj.Email,
                Password = PasswordHelper.ConvertToEncrypt(reqObj.Password),
                RoleId = reqObj.roleId
            };
        }
    }
}
