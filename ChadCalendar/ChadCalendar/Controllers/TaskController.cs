using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ChadCalendar.Controllers
{
    public class TaskController : Controller
    {
        private IEnumerable<Project> getProjects(User user)
        {
            var db = new ApplicationContext();
            return db.Projects.Where(proj => proj != null && proj.User == user);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddTask()
        {
            var db = new ApplicationContext();
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            ViewBag.Projects = getProjects(user);
            Models.Task task = new Models.Task();
            task.Name = "";
            task.NRepetitions = 1;
            task.MaxPerDay = 1;
            return View(task);
        }
        [HttpPost]
        public IActionResult AddTask(Models.Task task)
        {
            using (var db = new ApplicationContext())
            {
                if (!task.IsCorrect())
                    return View(task);
                User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                ViewBag.Projects = getProjects(user);

                task.User = user;
                task.Accessed = DateTime.Now;
                task.NRepetitions = 1;
                db.Add(task);
                db.SaveChanges();
            }
            return View(task);
        }
    }
}
