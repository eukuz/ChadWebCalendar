using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlazorChadCalendar.Data.Services
{
    public class TaskService
    {
        ApplicationContext db = new ApplicationContext();
        private void removeDependencies(Data.Task task)
        {
            var PredecessorDependecies = db.Tasks.Where(t => t.Predecessor == task);
            foreach (var item in PredecessorDependecies)
            {
                item.Predecessor = null;
                db.Tasks.Update(item);
                db.SaveChanges();
            }
        }
        public IEnumerable<Project> GetProjects(User user)
        {
            return db.Projects.Where(proj => proj.User == user);
        }
        public Data.Task GetTask(int? id)
        {
            return db.Tasks.Include(t => t.Project).Include(t => t.Predecessor).FirstOrDefault(t => t.Id == id);
        }
        public IEnumerable<Data.Task> GetTasks(User user)
        {
            return db.Tasks.Where(task => task.User == user);
        }
        public Data.Task GetPredecessor(int? id)
        {
            if (id != null)
                return db.Tasks.FirstOrDefault(t => t.Id == id);
            else
                return null;
        }
        public Data.User GetUser()
        {
            return db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
        }
        public void AddTask(Data.Task task, int projectId)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
            task.User = user;
            //task.Predecessor = getPredecessor(task.Predecessor.Id);
            task.Accessed = DateTime.Now;
            task.NRepetitions = 1;
            task.Predecessor = GetPredecessor(91);
            task.Project = db.Projects.FirstOrDefault(p => p.Id == projectId); // это странное выражение нужно потому что в модели передается только Id
            db.Add(task);
            db.SaveChanges();
        }
        public async void Edit(Data.Task task, int projectId)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
            task.Accessed = DateTime.Now;
            task.Project = db.Projects.FirstOrDefault(p => p.Id == projectId); // это странное выражение нужно потому что в модели передается только Id
            //Models.Task tempTask = task.Predecessor;
            task.Predecessor = GetPredecessor(91/*task.Predecessor.Id*/);
            db.Tasks.Update(task);
            await db.SaveChangesAsync();
        }
        public async void Delete(Data.Task task)
        {
            if (task != null)
            {
                task.Project = db.Projects.FirstOrDefault(p => p.Id == task.Project.Id);
                removeDependencies(task);
                db.Tasks.Remove(task);
                await db.SaveChangesAsync();
            }
        }
    }
}


//34 строчка в TaskAdd and TaskEdit (вырезано т.к дает ошибку)
//< div class= "form-group" >
 
//     < label > После какой задачи расположить</label>
//    <select @bind="task.Predecessor.Id">
//        <option value="@null">После любой</option>
//        @if (userProjects != null)
//        {
//    foreach (var task in userProjects)
//    {
//                < option value = "@task.Id" > @task.Name </ option >
//            }
//}
//    </ select >
//</ div >