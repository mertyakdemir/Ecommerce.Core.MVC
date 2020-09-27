using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.UI.Email;
using Ecommerce.UI.Identity;
using Ecommerce.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private IEmail emailstmp;
        private ICartBusiness cartBusiness;

        public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager, IEmail _emailstmp, ICartBusiness _cartBusiness)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            emailstmp = _emailstmp;
            cartBusiness = _cartBusiness;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel register_user)
        {
            if (!ModelState.IsValid)
            {
                return View(register_user);
            }

            var user = new User
            {
                UserName = register_user.UserName,
                FirstName = register_user.FirstName,
                LastName = register_user.LastName,
                Email = register_user.Email
            };

            ModelState.AddModelError("Password", "Your password must be at least 6 characters and contain numeric characters, lowercase characters, and special character.");

            var result = await userManager.CreateAsync(user, register_user.Password);

            if (result.Succeeded)
            {
                var cod = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = cod,
                });

                await emailstmp.SendEmailAsync(register_user.Email, "Please confirm account", $"Please <a href='https://localhost:44374{url}'>click</a> the link for account verify");

                return RedirectToAction("Login", "Account");
            }

            return View(register_user);
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login_user)
        {
            if(!ModelState.IsValid)
            {
                return View(login_user);
            }

            var user = await userManager.FindByEmailAsync(login_user.Email);

            if(user==null)
            {
                ModelState.AddModelError("", "No account registered with this email.");
                return View(login_user);
            }

            if(!await userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Please confirm membership.");
                return View(login_user);
            }

            var result = await signInManager.PasswordSignInAsync(user, login_user.Password, false, false);

            if(result.Succeeded)
            {
                return Redirect(login_user.ReturnUrl ?? "~/");
            }

            ModelState.AddModelError("", "Email or password is incorrect.");
            return View(login_user);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId == null || token == null)
            {
                TempData["message"] = "Error";
                return View();
            }

            var user = await userManager.FindByIdAsync(userId);

            if (userId != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    cartBusiness.InitializerCart(user.Id);
                    TempData["message"] = "Confirm Success";
                    return View();
                }
            }

            TempData["message"] = "Error";
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                return View();
            }

            var user = await userManager.FindByEmailAsync(email);

            if(user==null)
            {
                return View();
            }

            var cod = await userManager.GeneratePasswordResetTokenAsync(user);

            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = cod,
            });

            await emailstmp.SendEmailAsync(email, "Reset Password", $"<a href='https://localhost:44374{url}'>Click</a> the link for new password");

            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            if(userId==null || token==null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordModel { Token = token };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if(user==null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if(result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }
    }
}   