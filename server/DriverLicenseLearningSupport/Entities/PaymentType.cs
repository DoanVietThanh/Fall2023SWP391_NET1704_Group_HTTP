using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int PaymentTypeId { get; set; }
        public string PaymentTypeDesc { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
