using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Models
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and comfirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
