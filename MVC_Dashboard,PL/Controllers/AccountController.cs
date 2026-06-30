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
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel ViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(ViewModel.Email);

                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user,ViewModel.Password);

                    if (flag is true)
                    {
                        var result = await _SignInManager.PasswordSignInAsync(user, ViewModel.Password, ViewModel.RememberMe, false);

                        if (result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "This Email Is Locked!!");

                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    ModelState.AddModelError(string.Empty, "InCorrect Password");
                }
                else
                    ModelState.AddModelError(string.Empty, "No Account With This Email");
            }
            return View(ViewModel);
            }

    }
}
