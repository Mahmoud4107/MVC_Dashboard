using System.ComponentModel.DataAnnotations;

namespace MVC_Dashboard_PL.ViewModels.User
{
    public class SignInViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[MinLength(5 , ErrorMessage = "Minumum password length is 5")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
