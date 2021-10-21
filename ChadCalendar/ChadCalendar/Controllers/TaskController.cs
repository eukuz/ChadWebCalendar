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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTask(Models.Task task)
        {
            using (var db = new ApplicationContext())
            {
                User u = new User() { Login = "user1", Password = "123", };
                Project p = new Project() { Name="MainProject", User= u};
                task.Project = p;
                task.User = u;
                task.Accessed = DateTime.Now;
                task.Multiplier = 1;
                db.Add(task);
                db.SaveChanges();
            }
            return View();
        }
    }
}
