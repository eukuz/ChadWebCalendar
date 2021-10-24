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

        [Authorize]
        public IActionResult AddProject(int? id)
        {
            //if (id != null)
            //{
            //    Project p;
            //    using (var db = new ApplicationContext())
            //    {
            //        p = db.Projects.FirstOrDefault(p => p.Id == id);
            //    }
            //    return View(p);
            //}
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddProject(Models.Project project)
        {
            
            using (var db = new ApplicationContext())
            {
                project.User = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                if (project.Id == null) db.Add(project);
                else db.Update(project); //bag
                db.SaveChanges();
            }
            return View();
            
        }
    }
}
