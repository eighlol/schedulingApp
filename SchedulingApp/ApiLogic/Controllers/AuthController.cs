using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Controllers
{

    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly SignInManager<ScheduleAppUser> _signInManager;

        public AuthController(SignInManager<ScheduleAppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, true, false);
                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Index", "App");
                    }

                    return Redirect(returnUrl);
                }

                ModelState.AddModelError("", "Username or password incorrect");
            }

            return View();
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Login", "Auth");
        }

    }
}
