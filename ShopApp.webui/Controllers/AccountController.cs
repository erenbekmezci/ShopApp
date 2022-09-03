using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.business.Abstract;
using ShopApp.webui.EmailServices;
using ShopApp.webui.Identity;
using ShopApp.webui.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.webui.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager; //kullanıcı oluşturma falan login
        private SignInManager<User> _signInManager;//seession ve cookie
        private IEmailSender _emailSender;
        private ICartService _cartService;


     
        public AccountController(ICartService cartService,IEmailSender emailSender, UserManager<User> userManager , SignInManager<User> signInManager)
        {
            _userManager = userManager; //parola sıfırlama kullanıcı oluşturma login
            _signInManager = signInManager; //ccookie işlemleri için
            _emailSender = emailSender;
            _cartService = cartService;
        }
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel() { ReturnUrl = ReturnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce hesap oluşturulmamış.");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen mail hesabınıza gelen linkten maili onaylayınız.");

                return View(model);
            }
            
            var result = await _signInManager.PasswordSignInAsync(user, model.Password,false , false );

            if(result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }
            ModelState.AddModelError("", "Bu kullanıcı adı veya parola yanlış.");

            return View(model);

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email //password usermanager ile alcaz çünkü hashlenecek şifrelenecek
            };
            var result = await _userManager.CreateAsync(user , model.Password);
            if(result.Succeeded) //kullanıcı oluşturulduysa
            {
                // generate token  kullanıcıya token oluşturlması
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });

                //email ile göndercez ancak şuan confirmedemail false yani direkt aktif user olarak oluşturucak
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız", $"lütfen email hesabınızı onaylamak için linke <a href='http://localhost:40483{url}' >tıklayınız.</a>"); 
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "bilinmeyen bir hata oluştu tekrar deneyiniz.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }
        public async Task<IActionResult> ConfirmEmail(string userId , string token)
        {
            if(userId == null || token == null)
            {
                TempData["message"] = "geçersiz token";
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded)
                {
                    _cartService.InitializeCart(user.Id);
                    TempData["message"] = "Hesabınız onaylandı";
                    return View();
                }
            }
            TempData["message"] = "Hesabınız onaylanmadı";
            return View();
        }
        public  IActionResult ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if(string.IsNullOrEmpty(Email))
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
                return View();

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });

            //email ile göndercez ancak şuan confirmedemail false yani direkt aktif user olarak oluşturucak
            await _emailSender.SendEmailAsync(Email, "Reset password", $"parolayı sıfırlamak  için linke <a href='http://localhost:40483{url}' >tıklayınız.</a>");
            TempData["message"] = "Email gönderildi lütfen onaylayınız.";
            return View();
        }

        public IActionResult ResetPassword(string userId , string token)
        {
            if(userId == null || token == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var model = new ResetPasswordModel { Token = token };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Login" , "Account");
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }




    }
}
