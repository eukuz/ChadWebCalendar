using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadWebCalendar.Data.Services
{
    public class ProjectService
    {
        ApplicationContext db = new ApplicationContext();
        string tempLogin = "defourtend";
        public Data.Project GetProjectById(int? id)
        {
            if (id != null)
                return db.Projects.FirstOrDefault(p => p.Id == id);
            else
                return null;
        }
        public IEnumerable<Project> GetProjects(User user)
        {
            return db.Projects.Where(proj => proj.User == user);
        }
        public Data.User GetUser()
        {
            return db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
        }

        public async void Create(Project project)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == tempLogin/*User.Identity.Name*/);
            project.User = user;
            project.Accessed = DateTime.Now;
            db.Projects.Add(project);
            await db.SaveChangesAsync();
        }
        public async void Edit(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == tempLogin/*User.Identity.Name*/);
            if (id != null)
            {
                Project project = await db.Projects.Include(e => e.User).FirstOrDefaultAsync(p => p.Id == id);
                db.Projects.Update(project);
                await db.SaveChangesAsync();
            }
        }
        public async void Delete(int? id)
        {
            if (id != null)
            {
                Project project = db.Projects.FirstOrDefault(p => p.Id == id);
                if (project != null)
                {
                    var projectDependencies = db.Tasks.Where(t => t.Project == project);
                    foreach (var item in projectDependencies)
                    {
                        item.Project = null;
                    }
                    db.SaveChanges();
                    db.Projects.Remove(project);
                    db.SaveChanges();
                }
            }
        }
    }
}
