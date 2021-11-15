using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChadCalendar.Data.Services
{
    public class ProjectService
    {
        ApplicationContext db = new ApplicationContext();
      
        public Data.User GetUser()
        {
            return db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
        }
        public void CreateProject(Data.Project project)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
            project.User = user;
            project.Accessed = DateTime.Now;
            db.Add(project);
            db.SaveChanges();
        }
        public async void Edit(Data.Project project)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
            project.Accessed = DateTime.Now;
            db.Projects.Update(project);
            await db.SaveChangesAsync();
        }
    }
}
