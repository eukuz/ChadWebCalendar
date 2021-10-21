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
            return View();
        }
        [HttpPost]
        public IActionResult AddEvent(Event _event)
        {
            using (var db = new ApplicationContext())
            {
                User u = new User() { Login = "user1", Password = "123", };
                db.Add(u);
                db.SaveChanges();
                _event.User = u;
                _event.Accessed = DateTime.Now;
                _event.Multiplier = 1;
                db.Add(_event);
                db.SaveChanges();
            }
            return View();
        }

    }
}
