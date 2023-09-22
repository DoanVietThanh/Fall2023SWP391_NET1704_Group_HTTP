using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Utils;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class MemberAddRequest
    {
        // member account
        [Required(ErrorMessage = "Please input Email")]
        [EmailAddress(ErrorMessage = "Wrong email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please input Password")]
        public string Password { get; set; } = null!;


        // Member info
        [Required(ErrorMessage = "Please input First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Please input Last Name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Please input Date of Birth")]
        public string DateBirth { get; set; } = null!;

        [Required(ErrorMessage = "Please input Phone")]
        public string Phone { get; set; } = null!;


        // Address
        [Required(ErrorMessage = "Please input Street")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Please input Disctrict")]
        public string District { get; set; } = null!;

        [Required(ErrorMessage = "Please input City")]
        public string City { get; set; } = null!;

        // License Type
        public int LicenseTypeId { get; set; }
    }
    
    public static class MemberAddRequestExtension 
    {
        public static AccountModel ToAccountModel(this MemberAddRequest reqObj)
        {
            return new AccountModel
            {
                Email = reqObj.Email,
                Password = PasswordHelper.ConvertToEncrypt(reqObj.Password),
                RoleId = 4,
                IsActive = true
            };
        }
        public static MemberModel ToMemberModel(this MemberAddRequest reqObj, string defaultAvatar, string formatDate) 
        {
            return new MemberModel
            {
                FirstName = reqObj.FirstName,
                LastName = reqObj.LastName,
                DateBirth = DateTime.ParseExact(reqObj.DateBirth, formatDate,
                    CultureInfo.InvariantCulture),
                Phone = reqObj.Phone,
                LicenseTypeId = reqObj.LicenseTypeId,
                AvatarImage = defaultAvatar,
                IsActive = true
            };
        }

        public static AddressModel ToAddressModel(this MemberAddRequest reqObj)
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
