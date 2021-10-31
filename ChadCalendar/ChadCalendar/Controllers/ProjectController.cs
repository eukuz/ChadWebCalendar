using ChadCalendar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index()
        {
            return View(await db.Projects.ToListAsync());
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
            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Project project = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                    return View(project);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Project project = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                    return View(project);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Project project)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            project.User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            project.Accessed = DateTime.Now;
            db.Projects.Update(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Project project = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                    return View(project);
            }
            return NotFound();
        }

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
