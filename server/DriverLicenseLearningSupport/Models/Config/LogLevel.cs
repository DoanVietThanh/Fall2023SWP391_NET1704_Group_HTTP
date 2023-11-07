using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriverLicenseLearningSupport.Models.Config
{
    public class LogLevel
    {
        public string Default { get; set; }


        [DisplayName("Microsoft.AspNetCore")]
        [Column("Microsoft.AspNetCore")]
        public string Microsoft_AspNetCore { get; set; }


    }
}
