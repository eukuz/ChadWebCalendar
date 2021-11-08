using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
using ChadCalendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace ChadCalendar.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
        public AccountController(ApplicationContext context) { db = context; }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Login and/or password is incorrect");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    db.Users.Add(new User
                    {
                        Login = model.Login,
                        Password = model.Password,
                        TimeZone = model.TimeZone,
                        WorkingHoursFrom = model.WorkingHoursFrom,
                        WorkingHoursTo = model.WorkingHoursTo,
                        RemindEveryNDays = 5
                    });
                    await db.SaveChangesAsync();
                    await Authenticate(model.Login);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "User with this login already exists");
            }
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);
            EditModel userDetails = new EditModel()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                ConfirmPassword = user.Password,
                TimeZone = user.TimeZone,
                WorkingHoursFrom = user.WorkingHoursFrom,
                WorkingHoursTo = user.WorkingHoursTo,
                RemindMe = user.RemindEveryNDays>0,
                RemindEveryNDays = user.RemindEveryNDays > 0 ? user.RemindEveryNDays : 0
            };
            ViewBag.User = user;
            return View(userDetails);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditModel model)
        {
            if (model.Password != null)
            {

                if (ModelState.IsValid)
                {

                    User user = await db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                    if (user != null)
                    {
                        user.Login = model.Login;
                        user.Password = model.ConfirmPassword.Length > 0 ? model.ConfirmPassword : user.Password;
                        user.TimeZone = model.TimeZone;
                        user.WorkingHoursFrom = model.WorkingHoursFrom;
                        user.WorkingHoursTo = model.WorkingHoursTo;
                        user.RemindEveryNDays = 5;
                        user.RemindEveryNDays = model.RemindMe ? model.RemindEveryNDays : -1;

                        db.Users.Update(user);
                        await db.SaveChangesAsync();
                        await Authenticate(model.Login);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", "User does not exist! Some error happend with DB");
                }
            }
            return View(model);
        }


        private async System.Threading.Tasks.Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
