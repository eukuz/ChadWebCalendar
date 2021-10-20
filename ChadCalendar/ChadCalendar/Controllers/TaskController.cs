using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
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
                db.Add(task);
                db.SaveChanges();
            }
            return View();
        }
    }
}
