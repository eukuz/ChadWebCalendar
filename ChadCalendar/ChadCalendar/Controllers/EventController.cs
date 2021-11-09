using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ChadCalendar.Controllers
{
    public class EventController : Controller
    {
        private ApplicationContext db;

        public EventController(ApplicationContext context)
        {
            db = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await db.Events.Where(p => p.User.Login == User.Identity.Name).ToListAsync());
        }

        [Authorize]
        public IActionResult Create()
        {
            Event _event = new Event();
            DateTime dt = DateTime.Now;
            _event.StartsAt = dt.Date.AddHours(dt.Hour).AddMinutes(dt.Minute);
            dt = dt.AddMinutes(10);
            _event.FinishesAt = dt.Date.AddHours(dt.Hour).AddMinutes(dt.Minute);
            return View(_event);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Event _event)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            _event.User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            _event.Accessed = DateTime.Now;
            _event.NRepetitions = 1;
            if (!_event.IsCorrect())
            {
                ViewBag.Error = true;
                return View(_event);
            }
            db.Events.Add(_event);
            await db.SaveChangesAsync();
            return Redirect("~/");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (id != null)
            {
                Event _event = await db.Events.FirstOrDefaultAsync(p => p.Id == id);
                if (_event != null && _event.User == user)
                    return View(_event);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Event _event)
        {
            _event.User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            _event.Accessed = DateTime.Now;
            if (!_event.IsCorrect())
            {
                ViewBag.Error = true;
                return View(_event);
            }
            db.Events.Update(_event);
            await db.SaveChangesAsync();
            return Redirect("~/");
        }

        [Authorize]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Event _event = await db.Events.FirstOrDefaultAsync(p => p.Id == id);
                if (_event != null)
                    return View(_event);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Event _event = await db.Events.FirstOrDefaultAsync(p => p.Id == id);
                if (_event != null)
                {
                    db.Events.Remove(_event);
                    await db.SaveChangesAsync();
                    return Redirect("~/");
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> Mutatuion(Models.Event _event)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            _event = await db.Events.FirstOrDefaultAsync(t => _event.Id == t.Id);
            Models.Task task = new Models.Task(_event, null);
            task.User = user;
            db.Tasks.Add(task);
            db.Events.Remove(_event);
            await db.SaveChangesAsync();
            return Redirect("~/");
        }
    }
}
