using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACarProject.Models;
using RentACarProject.Models.UserModels;
// Diğer using ifadeleri...

namespace RentACarProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        // Login ve Register formunu gösteren action
        public IActionResult LoginRegister()
        {
            return View();
        }
        public IActionResult MyAccount()
        {
            return View();
        }

        // POST: Login işlemi
        [HttpPost]
        public async Task<IActionResult> Login(LoginRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.LoginViewModel.Email, model.LoginViewModel.Password, model.LoginViewModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("LoginRegister", model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.RegisterViewModel.Email, Email = model.RegisterViewModel.Email };
                var result = await _userManager.CreateAsync(user, model.RegisterViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("LoginRegister", model);
        }


        // POST: Şifremi Unuttum
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Şifre sıfırlama işlemini başlatın
                // ...

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View("LoginRegister", model);
        }

        // Şifre Sıfırlama Onayı
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // POST: Şifre Sıfırla
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Şifre sıfırlama işlemini tamamlayın
                // ...

                return RedirectToAction("ResetPasswordConfirmation");
            }

            return View("LoginRegister", model);
        }

        // Şifre Sıfırlama Onayı
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
