using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using ChadCalendar.TaskHandlers;

namespace ChadCalendar.Controllers
{
    public class TaskController : Controller
    {
        ApplicationContext db = new ApplicationContext();
        ErrorsHandler errorsHandler = new ErrorsHandler();
        private IEnumerable<Project> getProjects(User user)
        {
            return db.Projects.Where(proj => proj.User == user);
        }
        private IEnumerable<Models.Task> getTasks(User user)
        {
            return db.Tasks.Where(task => task.User == user);
        }
        private Models.Task getPredecessor(int? id)
        {
            if (id != null)
                return db.Tasks.FirstOrDefault(t => t.Id == id);
            else
                return null;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            return View(await db.Tasks.Where(task => task.User == user).ToListAsync());
        }

        [Authorize]
        public IActionResult AddTask()
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            ViewBag.Projects = getProjects(user);
            ViewBag.TasksOfProject = getTasks(user);
            Models.Task task = new Models.Task();
            return View(task);
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddTask(Models.Task task)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            ViewBag.Projects = getProjects(user);
            task.User = user;
            task.Predecessor = getPredecessor(task.Predecessor.Id);
            task.Accessed = DateTime.Now;
            task.NRepetitions = 1;
            task.Project = db.Projects.FirstOrDefault(p => p.Id == task.Project.Id);
            if (!task.IsCorrect())
            {
                ViewBag.Error = true;
                ViewBag.TasksOfProject = getTasks(user);
                return View(task);
            }
            db.Add(task);
            db.SaveChanges();
            return Redirect("~/");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            ViewBag.Projects = getProjects(user);
            ViewBag.TasksOfProject = getTasks(user); 
            if (id != null)
            {
                Models.Task task = await db.Tasks.Include(t => t.Project).Include(t => t.Predecessor).FirstOrDefaultAsync(p => p.Id == id);


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
            task.Project = db.Projects.FirstOrDefault(p => p.Id == task.Project.Id);
            task.Predecessor = getPredecessor(task.Predecessor.Id);
            ViewBag.Projects = getProjects(user);
            if (!task.IsCorrect())
            {
                ViewBag.Error = true;
                ViewBag.TasksOfProject = getTasks(user);
                return View(task);
            }
            db.Tasks.Update(task);
            await db.SaveChangesAsync();
            return Redirect("~/");
            //return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Models.Task task = await db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
                if (task != null)
                {
                    db.Tasks.Remove(task);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
