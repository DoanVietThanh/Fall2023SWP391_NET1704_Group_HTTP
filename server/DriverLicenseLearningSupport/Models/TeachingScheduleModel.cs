using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class TeachingScheduleModel
    {
        public int TeachingScheduleId { get; set; }
        public DateTime TeachingDate { get; set; }
        public string StaffId { get; set; }
        public int? SlotId { get; set; }
        public int? VehicleId { get; set; }
        public int? WeekdayScheduleId { get; set; }
        public string? CoursePackageId { get; set; }

        public virtual CoursePackageModel? CoursePackage { get; set; }

        //public virtual SlotModel Slot { get; set; }
        public virtual StaffModel? Staff { get; set; }
        public virtual VehicleModel? Vehicle { get; set; }
        //public virtual WeekdayScheduleModel WeekdaySchedule { get; set; }
        public virtual ICollection<RollCallBookModel> RollCallBooks { get; set; } = new List<RollCallBookModel>();
    }
}
