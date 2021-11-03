using ChadCalendar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationContext db;
        public ProjectController(ApplicationContext context)
        {
            db = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await db.Projects.Where(p => p.User.Login == User.Identity.Name).ToListAsync());
        }

        [Authorize]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            project.User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            project.Accessed = DateTime.Now;

            if (string.IsNullOrEmpty(project.Name))
            {
                ModelState.AddModelError("Name", "Введите имя");
            }

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (id != null)
            {
                Project project = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null && project.User == user)
                    return View(project);
            }
            return NotFound();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Project project)
        {
            project.User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            project.Accessed = DateTime.Now;
            db.Projects.Update(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Project project = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                {
                    db.Projects.Remove(project);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
