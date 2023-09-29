using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Utils;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class StaffUpdateRequest
    {
        // Staff Info
        [Required(ErrorMessage = "Vui lòng nhập Họ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Tên")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public string DateBirth { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        // avatar image
        public IFormFile AvatarImage { get; set; }


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
    }

    public static class StaffUpdateRequestExtension
    {
        public static StaffModel ToStaffModel(this StaffUpdateRequest reqObj, string formatDate)
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
            };
        }

        public static AddressModel ToAddressModel(this StaffUpdateRequest reqObj)
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
