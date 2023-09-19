using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Utils;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class RegisterRequest
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
    }

    public static class RegisterRequestExtension
    {
        public static AccountModel ToAccountModel(this RegisterRequest reqObj)
        {
            return new AccountModel
            {
                Email = reqObj.Username,
                Password = PasswordHelper.ConvertToEncrypt(reqObj.Password),
                RoleId = 4,
                IsActive = true
            };
        }

        public static MemberModel ToMemberModel(this RegisterRequest reqObj, string formatDate)
        {
            return new MemberModel
            {
                FirstName = reqObj.FirstName,
                LastName = reqObj.LastName,
                DateBirth = DateTime.ParseExact(reqObj.DateBirth, formatDate,
                CultureInfo.InvariantCulture),
                Phone = reqObj.Phone,
                LicenseTypeId = reqObj.LicenseTypeId,
            };
        }

        public static AddressModel ToAddressModel(this RegisterRequest reqObj)
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

