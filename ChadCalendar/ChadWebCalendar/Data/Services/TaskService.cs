using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace ChadWebCalendar.Data.Services
{
    public class TaskService
    {
        ApplicationContext db = new ApplicationContext();
        public bool IsCorrect(in Data.Task task)
        {
            if (task.Name != null && task.TimeTakes != null && task.Name != "")
                return true;
            else
                return false;
        }
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
        public Data.Project GetSelectedProject(Data.User _user)
        {
            ApplicationContext db1 = new ApplicationContext();
            return db1.Projects.FirstOrDefault(p => (p.User.Id == _user.Id) && (p.Id == _user.SelectedProjectId));
        }
        public Data.Project GetFirstProject(Data.User _user)
        {
            return db.Projects.FirstOrDefault(p => p.User.Id == _user.Id);
        }
        public Data.Task GetTask(int? id)
        {
            return db.Tasks.Include(t => t.Project).Include(t => t.Predecessor).FirstOrDefault(t => t.Id == id);
        }
        public IEnumerable<Data.Task> GetTasks(User user)
        {
            ApplicationContext db1 = new ApplicationContext();
            return db1.Tasks.Include(t =>t.Project).Where(task => task.User == user);
        }
        public Data.Task GetPredecessor(int? id)
        {
            if (id != null)
                return db.Tasks.FirstOrDefault(t => t.Id == id);
            else
                return null;
        }
        public Data.User GetUser(string Name)
        {
            ApplicationContext db1 = new ApplicationContext();
            return db1.Users.FirstOrDefault(u => u.Login == Name);
        }
        public bool AddTask(Data.Task task, int? projectId, User user)
        {
            task.User = user;
            task.Accessed = DateTime.Now;
            task.NRepetitions = 1;
            task.Predecessor = GetPredecessor(91);
            task.Project = db.Projects.FirstOrDefault(p => p.Id == projectId);
            if (String.IsNullOrWhiteSpace(task.Name))
            {
                task.Name = "(Нет заголовка)";
            }
            if (IsCorrect(task))
            {
                db.Add(task);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public bool Edit(Data.Task task, int projectId, string Name)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == Name);
            task.Accessed = DateTime.Now;
            task.Project = db.Projects.FirstOrDefault(p => p.Id == projectId); // это странное выражение нужно потому что в модели передается только Id
            //Models.Task tempTask = task.Predecessor;
            task.Predecessor = GetPredecessor(91/*task.Predecessor.Id*/);
            if (IsCorrect(task))
            {
                db.Tasks.Update(task);
                db.SaveChangesAsync();
                return true;
            }
            return false;
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
        public async void Mutatuion(int? id, string Name, DateTime StartsAt, DateTime FinishesAt)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == Name);
            Data.Task task = db.Tasks.FirstOrDefault(t => id == t.Id); // это странное выражение нужно потому что в модели передается только Id
            removeDependencies(task); // избавляемся от зависимостей
            Event _event = new Event(task, StartsAt, FinishesAt, 15);
            _event.User = user;
            db.Events.Add(_event);
            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
        }
    }
}


//34 строчка в TaskAdd and TaskEdit (вырезано т.к дает ошибку)
