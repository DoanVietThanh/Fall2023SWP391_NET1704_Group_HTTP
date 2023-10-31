using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class SlotUpdateRequest
    {
        [Required(ErrorMessage = "Slot name is required")]
        public string SlotName { get; set; }

        [Required(ErrorMessage = "Slot duration is required")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Hour is required")]
        public int Hours { get; set; }

        [Required(ErrorMessage = "Minute is required")]
        public int Minutes { get; set; }
    }

    public static class SlotUpdateRequestExtension
    {
        public static SlotModel ToSlotModel(this SlotUpdateRequest reqObj) 
        {
            return new SlotModel
            {
                SlotName = reqObj.SlotName,
                Duration = reqObj.Duration,
                Time = new TimeSpan(reqObj.Hours, reqObj.Minutes, 0)
            };
        }
    }
}
