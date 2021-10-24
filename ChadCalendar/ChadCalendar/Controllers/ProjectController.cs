using ChadCalendar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Controllers
{
    public class ProjectController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult AddProject()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddProject(Models.Project project)
        {
            using (var db = new ApplicationContext())
            {
                project.User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                db.Add(project);
                db.SaveChanges();
            }
            return View();
            
        }
    }
}
