using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class MemberModel
    {
        public string MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public string AvatarImage { get; set; }
        public string AddressId { get; set; }
        public string Email { get; set; }
        public int? LicenseFormId { get; set; }
        public virtual AddressModel Address { get; set; }
        public virtual AccountModel EmailNavigation { get; set; }
        public virtual LicenseRegisterFormModel? LicenseForm { get; set; }
    }
}