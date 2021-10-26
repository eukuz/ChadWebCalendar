using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEvent()
        {
            Event _event = new Event();
            DateTime dt = DateTime.Now;
            _event.StartsAt = dt.Date.AddHours(dt.Hour).AddMinutes(dt.Minute);
            dt = dt.AddMinutes(10);
            _event.FinishesAt = dt.Date.AddHours(dt.Hour).AddMinutes(dt.Minute);
            _event.Description = "";
            _event.Name = "";
            return View(_event);
        }
        [HttpPost]
        public IActionResult AddEvent(Event _event)
        {
            using (var db = new ApplicationContext())
            {
                if (!_event.IsCorrect())
                {
                    return View(_event);
                }
                User u = new User() { Login = "user1", Password = "123", };
                db.Add(u);
                db.SaveChanges();
                _event.User = u;
                _event.Accessed = DateTime.Now;
                _event.NRepetitions = 1;
                db.Add(_event);
                db.SaveChanges();
            }
            return View(_event);
        }

    }
}
