namespace DriverLicenseLearningSupport.Payloads.Filters
{
    public class StaffFilter
    {
        public string? Name { get; set; } = null!;
        public string? DateBirth { get; set; } = null!;
        public string? Phone { get; set; } = null!;
        public string? Street { get; set; } = null!;
        public string? District { get; set; } = null!;
        public string? City { get; set; } = null!;
        public int? RoleId { get; set; } = null!;
        public int? JobTitleId { get; set; } = null!;
        public int? LicenseTypeId { get; set; } = null!;
    }
}
