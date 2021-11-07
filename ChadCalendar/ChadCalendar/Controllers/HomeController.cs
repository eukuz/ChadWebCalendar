using ChadCalendar.Models;
using ChadCalendar.ViewModels;
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
        private ApplicationContext db;

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        [Authorize]
        public IActionResult Index(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            var model = new FooViewModel();
            if (id.HasValue)
            {
                model.Tasks = db.Tasks.Where(t => t.Project.Id == id).ToList();
            }
            else
            {
                model.Tasks = db.Tasks.Where(t => t.User == user).ToList();
            }

            model.Events = db.Events.Where(e => e.User == user).ToList();
            model.Projects = db.Projects.Where(p => p.User == user).ToList();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
