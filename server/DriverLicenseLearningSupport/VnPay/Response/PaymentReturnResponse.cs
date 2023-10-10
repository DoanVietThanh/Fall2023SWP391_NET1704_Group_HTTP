using Amazon.Runtime.Internal;
using DriverLicenseLearningSupport.Utils;
using System.Net;
using System.Text;
using VNPayPackage.Enums;

namespace DriverLicenseLearningSupport.VnPay.Response
{
    public class PaymentReturnResponse
    {
        public SortedList<string, string> responseData
            = new SortedList<string, string>();
        public string? PaymentId { get; set; }
        public string? PaymentStatus { get; set; }
        /// <summary>
        /// Format : yyyyyMMddHHmmss
        /// </summary>
        public string? PaymentDate { get; set; }
        public string? CourseReservationId { get; set; }
        public decimal? Ammount { get; set; }
        public string? Signature { get; set; }
   
    }
}
