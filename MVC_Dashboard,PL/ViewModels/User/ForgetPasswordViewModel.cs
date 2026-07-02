using System.ComponentModel.DataAnnotations;

namespace MVC_Dashboard_PL.ViewModels.User
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
