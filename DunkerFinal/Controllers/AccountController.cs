using Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Service.Helpers.Enums;
using Service.ViewModels.Account;

namespace DunkerFinal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM request)
        {

            request.Username = request.Username.Trim();
            request.Fullname = request.Fullname.Trim();
            if (!ModelState.IsValid) { return View(request); }
            AppUser user = new()
            {
                UserName = request.Username,
                Email = request.Email,
                FullName = request.Fullname
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(request);
            }

            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string url = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("tofigtn@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Register confirmation";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@" <!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Email Confirmation</title>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f0f0f0; margin: 0; padding: 0;'>
    <div style='max-width: 600px; margin: 20px auto; padding: 20px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
        <div style='background-color: #000000; color: white; text-align: center; padding: 20px 0; border-top-left-radius: 8px; border-top-right-radius: 8px;'>
            <h2 style='margin-bottom: 10px;'>Confirm Your Email Address</h2>
            <p style='font-size: 16px;'>Thank you for registering with Dunker!</p>
        </div>
        <div style='padding: 20px; text-align: center;'>
            <p style='font-size: 18px;'>Dear {user.UserName},</p>
            <p style='font-size: 16px;'>Please click the button below to confirm your email address:</p>
            <p>
                <a href='{url}' style='display: inline-block; padding: 12px 24px; background-color: #000000; color: white; text-decoration: none; border-radius: 5px; transition: background-color 0.3s ease;'>Confirm Email</a>
            </p>
            <p style='font-size: 16px;'>If you did not create an account with Dunker, please ignore this email.</p>
        </div>
        <div style='margin-top: 20px; color: #666666; text-align: center;'>
            <p style='font-size: 14px;'>Best regards,<br/>The Dunker Team</p>
            <p style='font-size: 12px;'>© {DateTime.Now.Year} Dunker. All rights reserved.</p>
            <p style='font-size: 12px; margin-top: 10px;'>You are receiving this email because you signed up for an account at <a href='https://dunker.com' style='color: #000000; text-decoration: none;'>dunker.com</a>. If you have any questions, please contact <a href='mailto:support@dunker.com' style='color: #000000; text-decoration: none;'>support@dunker.com</a>.</p>
        </div>
    </div>
</body>
</html>
"
            };



            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("tofigtn@code.edu.az", "ltos famy mrkc gxzw");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(VerifyEmail));
        }


        [HttpGet]
        public IActionResult VerifyEmail()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.ConfirmEmailAsync(user, token);
            return RedirectToAction(nameof(Login));
        }




        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM request)
        {
            if (!ModelState.IsValid) return View(request);

            AppUser existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser is null)
            {
                ModelState.AddModelError("Email", "User not found!");
                return View(request);
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(existUser);

            string link = Url.Action(nameof(ResetPassword), "Account", new { userId = existUser.Id, token }, Request.Scheme, Request.Host.ToString());

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("tofigtn@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(existUser.Email));
            email.Subject = "Reset Password";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Reset Password</title>
            </head>
            <body style='font-family: Arial, sans-serif; background-color: #f0f0f0; margin: 0; padding: 0;'>
                <div style='max-width: 600px; margin: 20px auto; padding: 20px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
                    <div style='background-color: #000000; color: white; text-align: center; padding: 20px 0; border-top-left-radius: 8px; border-top-right-radius: 8px;'>
                        <h2 style='margin-bottom: 10px;'>Reset Your Password</h2>
                        <p style='font-size: 16px;'>Click the button below to reset your password:</p>
                    </div>
                    <div style='padding: 20px; text-align: center;'>
                        <p style='font-size: 18px;'>Dear {existUser.FullName},</p>
                        <p style='font-size: 16px;'>To reset your password, please click the button below:</p>
                        <p>
                            <a href='{link}' style='display: inline-block; padding: 12px 24px; background-color: #000000; color: white; text-decoration: none; border-radius: 5px; transition: background-color 0.3s ease;'>Reset Password</a>
                        </p>
                        <p style='font-size: 16px;'>If you did not request a password reset, please ignore this email.</p>
                    </div>
                    <div style='margin-top: 20px; color: #666666; text-align: center;'>
                        <p style='font-size: 14px;'>Best regards,<br/>The Dunker Team</p>
                        <p style='font-size: 12px;'>© {DateTime.Now.Year} Dunker. All rights reserved.</p>
                        <p style='font-size: 12px; margin-top: 10px;'>If you have any questions, please contact <a href='mailto:support@dunker.com' style='color: #000000; text-decoration: none;'>support@dunker.com</a>.</p>
                    </div>
                </div>
            </body>
            </html>
        "
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("tofigtn@code.edu.az", "ltos famy mrkc gxzw");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(CheckEmail));
        }

        public IActionResult CheckEmail()
        {
            return View();
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordVM { Token = token, UserId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (!ModelState.IsValid) return View(resetPassword);

            AppUser existUser = await _userManager.FindByIdAsync(resetPassword.UserId);

            if (existUser == null) return NotFound();

            if (await _userManager.CheckPasswordAsync(existUser, resetPassword.Password))
            {
                ModelState.AddModelError("", "This password already exists!");
                return View(resetPassword);
            }

            await _userManager.ResetPasswordAsync(existUser, resetPassword.Token, resetPassword.Password);

            return RedirectToAction("Login", "Account");
        }




        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByEmailAsync(login.UserNameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(login.UserNameOrEmail);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Email, Username or Password is incorrect");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsRemember, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Server is eneabled at the moment,please try again later");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email, Username or Password is incorrect");
                return View();
            }


            await _signInManager.SignInAsync(user, login.IsRemember);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
