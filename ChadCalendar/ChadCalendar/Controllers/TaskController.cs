using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            return View(await db.Tasks.Where(task => task.User == user).ToListAsync());
        }

        [Authorize]
        public IActionResult Create()
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            ViewBag.Projects = getProjects(user);
            Models.Task task = new Models.Task();
            task.Name = "";
            task.NRepetitions = 1;
            task.MaxPerDay = 1;
            return View(task);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(Models.Task task)
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

        [Authorize]
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

        [Authorize]
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
