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
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


        // Member Info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateBirth { get; set; }
        public string Phone { get; set; }


        // Address Info
        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }
        [Required(ErrorMessage = "District is required")]
        public string District { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
    }

    public static class SignUpRequestExtension
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
                Phone = reqObj.Phone
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

