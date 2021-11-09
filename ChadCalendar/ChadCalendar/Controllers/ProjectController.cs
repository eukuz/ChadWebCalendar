using ChadCalendar.Models;
using ChadCalendar.ViewModels;
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
        public async Task<IActionResult> Create(CreateProjectModel _project)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Project project = new Project()
            {
                Name = _project.Name,
                Description = _project.Description,
                Accessed = DateTime.Now,
                Frequency = _project.Frequency,
                Deadline = _project.Deadline,
                IconNumber = _project.IconNumber,
                NRepetitions = _project.NRepetitions,
                User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name)
            };

            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (id != null)
            {
                Project project = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
                CreateProjectModel projectModel = new CreateProjectModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Accessed = project.Accessed,
                    Frequency = project.Frequency,
                    Deadline = project.Deadline,
                    IconNumber = project.IconNumber,
                    NRepetitions = project.NRepetitions,

                };
                if (project != null && project.User == user)
                    return View(projectModel);
            }
            return NotFound();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateProjectModel _project)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            _project.User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            _project.Accessed = DateTime.Now;


            Project project = db.Projects.FirstOrDefault(p => p.Id == _project.Id);
            project.Name = _project.Name;
            project.Description = _project.Description;
            project.Accessed = _project.Accessed;
            project.Frequency = _project.Frequency;
            project.Deadline = _project.Deadline;
            project.IconNumber = _project.IconNumber;
            project.NRepetitions = _project.NRepetitions;
            project.User = _project.User;

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
