using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVC_Dashboard.DAL.Models;
using MVC_Dashboard_PL.Services.SendEmail;
using MVC_Dashboard_PL.ViewModels.User;
using System.Threading.Tasks;

namespace MVC_Dashboard_PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        public SignInManager<ApplicationUser> _SignInManager { get; }

        public AccountController(IConfiguration configuration ,IEmailSender emailSender,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _emailSender = emailSender;
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
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(ViewModel.Email);

                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, ViewModel.Password);

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
        public async new Task<IActionResult> SignOut()
        {
           await _SignInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ResetPassword", "Account", new { Email = model.Email, Token = token },Request.Scheme);

                    //     http://localhost:43375/Account/ResetPassword?email="mahmoudhashem.cs@gmail.com"
                    //     &token = CfDJ8CRVc6wtm1VIr5C5smS3q / JQZgdK3QPQVltPL8P13pdXBX / 5xtDEZ9qHBkfbjNVW6zf + 6X9Z73
                    //     b8ge6tN9RSp1FtHdGUxT1hfHREiHTpowTFw2Qck / 8LHoarpdO8738bbOGKjalCeAE13jl2LCpmsGDY6
                    //     xfTOYFZxKftpxpT3DyYd4F1CKkj + ERH1aFLQjqRqA1h9purvO9f + 3jx1zIUvQemYeF1eN4FxOx / xHr6R3eE"

                    await _emailSender.SendEmailAsync(_configuration["EmailSender:Email"], model.Email, "Reset Password", url);

                    return RedirectToAction(nameof(CheckYourInbox));

                }

                ModelState.AddModelError(string.Empty, "Invalid Email");
            }
            return View(model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string Email,string token)
        {
            TempData["Email"] = Email;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var Email = TempData["Email"] as string;
                var token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(Email);

                if (user is not null)
                {

                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if(result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
                ModelState.AddModelError(string.Empty, "URL Is Not Valid");

            }
            return View();
        }




    }
}
