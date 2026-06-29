using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Dashboard.DAL.Models;
using MVC_Dashboard_PL.ViewModels.User;
using System.Threading.Tasks;

namespace MVC_Dashboard_PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public SignInManager<ApplicationUser> _SignInManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _SignInManager = signInManager;
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(ViewModel.Username);

                if (user is null)
                {
                    var model = new ApplicationUser()
                    {
                        UserName = ViewModel.Username,
                        Email = ViewModel.Email,
                        IsAgree = ViewModel.IsAgree,
                        FName = ViewModel.FirstName,
                        LName = ViewModel.LastName
                    };

                    var result = await _userManager.CreateAsync(model,ViewModel.Password);

                    if(result.Succeeded)
                         return RedirectToAction(nameof(SignIn));

                    foreach(var error in result.Errors)
                         ModelState.AddModelError(string.Empty, error.Description);
                }
                else
                    ModelState.AddModelError(string.Empty, "this username is already in use for another account");
            }

            return View(ViewModel);
                
        }
        public IActionResult SignIn()
        {
            return View();
        }

    }
}
