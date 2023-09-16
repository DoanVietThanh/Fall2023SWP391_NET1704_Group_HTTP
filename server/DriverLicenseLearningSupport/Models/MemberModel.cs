namespace DriverLicenseLearningSupport.Models
{
    public class MemberModel
    {
        public string MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public string? AddressId { get; set; }
        public string Email { get; set; }
        public int? LicenseTypeId { get; set; }
    }
}
