using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class MemberUpdateRequest
    {
        // Member Info
        [Required]
        public Guid MemberId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Birth date number is required")]
        public string DateBirth { get; set; }

        [Required(ErrorMessage = "Street number is required")]
        public string Street { get; set; }
        [Required(ErrorMessage = "District number is required")]
        public string District { get; set; }
        [Required(ErrorMessage = "City number is required")]
        public string City { get; set; }

        // License Type
        public int LicenseTypeId { get; set;}

    }

    public static class MemberUpDateRequestExtension
    {
        public static MemberModel ToMemberModel(this MemberUpdateRequest reqObj, string dateFormat) 
        {
            return new MemberModel
            {
                // privacy info
                FirstName = reqObj.FirstName,
                LastName = reqObj.LastName,
                Phone = reqObj.Phone,
                DateBirth = DateTime.ParseExact(reqObj.DateBirth, dateFormat,
                CultureInfo.InvariantCulture),
                // license type
                LicenseTypeId = reqObj.LicenseTypeId,
                // address info
                Address = new AddressModel 
                {
                    Street = reqObj.Street,
                    City = reqObj.City,
                    District = reqObj.District
                }
            };
        }
    }
}
