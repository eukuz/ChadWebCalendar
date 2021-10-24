using ChadCalendar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //public IActionResult AddUsers()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult AddUsers(User user)
        //{
        //    using(var db = new ApplicationContext())
        //    {
        //        //User u = new User() { Login = "user1", Password = "123", };
        //        //Project p = new Project() { Name="MainProject", User= u};
        //        //Models.Task task = new Models.Task() {User = u, Project= p};
        //        //db.Add(u);
        //        //db.Add(p);
        //        //db.Add(task);

        //        db.Add(user);
        //        db.SaveChanges();
        //    }
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
