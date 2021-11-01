using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ChadCalendar.Controllers
{
    public class TaskController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        private IEnumerable<Project> getProjects(User user)
        {
            return db.Projects.Where(proj => proj.User == user);
        }
        private IEnumerable<Models.Task> getTasks(User user)
        {
            return db.Tasks.Where(task => task.User == user);
        }

        public async Task<IActionResult> Index()
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            return View(await db.Tasks.Where(task => task.User == user).ToListAsync());
        }

        public IActionResult AddTask()
        {
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
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (!task.IsCorrect())
                 return View(task);
            ViewBag.Projects = getProjects(user);
            task.User = user;
            task.Accessed = DateTime.Now;
            task.NRepetitions = 1;
            db.Add(task);
            db.SaveChanges();
            return Redirect("~/Task/Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);

            if (id != null)
            {
                Models.Task task = await db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
                ViewBag.Projects = getProjects(user);

                if (task != null && task.User == user)
                    return View(task);

            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Models.Task task)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            task.Accessed = DateTime.Now;
            ViewBag.Projects = getProjects(user);
            db.Tasks.Update(task);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
