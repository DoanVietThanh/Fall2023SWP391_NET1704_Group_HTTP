using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Utils;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class StaffAddRequest
    {
        // Account Info
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }


        // Member Info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateBirth { get; set; }
        public string Phone { get; set; }
        public string? SelfDescription { get; set; }


        // Address Info
        [Required(ErrorMessage = "Vui lòng nhập Đường")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Quận")]
        public string District { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Thành Phố")]
        public string City { get; set; }

        // License Type
        [Required(ErrorMessage = "Vui lòng chọn loại bằng lái")]
        public int LicenseTypeId { get; set; }

        // Job Title
        [Required(ErrorMessage = "Vui lòng chọn loại công việc")]
        public int JobTitleId { get; set; }

        // Account Role
        [Required(ErrorMessage = "Vui lòng chọn Role")]
        public int RoleId { get; set; }
    }

    public static class StaffAddRequestExtension
    {
        public static AccountModel ToAccountModel(this StaffAddRequest reqObj)
        {
            return new AccountModel
            {
                Email = reqObj.Username,
                Password = PasswordHelper.ConvertToEncrypt(reqObj.Password),
                RoleId = reqObj.RoleId,
                IsActive = true
            };
        }

        public static StaffModel ToStaffModel(this StaffAddRequest reqObj, string formatDate)
        {
            return new StaffModel
            {
                FirstName = reqObj.FirstName,
                LastName = reqObj.LastName,
                DateBirth = DateTime.ParseExact(reqObj.DateBirth, formatDate,
                CultureInfo.InvariantCulture),
                Phone = reqObj.Phone,
                JobTitleId = reqObj.JobTitleId,
                LicenseTypeId = reqObj.LicenseTypeId,
                SelfDescription = WebUtility.UrlEncode(reqObj.SelfDescription)
            };
        }

        public static AddressModel ToAddressModel(this StaffAddRequest reqObj)
        {
            return new AddressModel
            {
                City = reqObj.City,
                District = reqObj.District,
                Street = reqObj.Street
            };
        }
    }
}
