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
        private IEnumerable<Project> getProjects()
        {
            var db = new ApplicationContext();
            return db.Projects.Where(proj => proj != null);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddTask()
        {

            ViewBag.Projects = getProjects();
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
                ViewBag.Projects = getProjects();
                if (!task.IsCorrect())
                    return View(task);
                User u = new User() { Login = "user1", Password = "123", };
                Project p = new Project() { Name="MainProject", User= u};
                task.Project = p;
                task.User = u;
                task.Accessed = DateTime.Now;
                task.Multiplier = 1;
                db.Add(task);
                db.SaveChanges();
                var listOfUserProjects = db.Projects.Where(Proj => Proj.Name == "NotMain");
            }
            return View(task);
        }
    }
}
