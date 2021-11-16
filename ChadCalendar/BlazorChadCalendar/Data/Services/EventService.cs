using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlazorChadCalendar.Data.Services
{
    public class EventService
    {
        ApplicationContext db = new ApplicationContext();
        string tempLogin = "defourtend";
        public Data.User GetUser()
        {
            return db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
        }
        public Data.Event GetEventById(int? id)
        {
            if (id != null)
                return db.Events.FirstOrDefault(e => e.Id == id);
            else
                return null;
        }
        public IEnumerable<Data.Event> GetEvents(int? userId)
        {
            if (userId != null)
            {
                return db.Events.Where(e => e.User.Id == userId);
            }
            else
                return null;
        }
        public async void Create(Event _event)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == tempLogin/*User.Identity.Name*/);
            _event.User = user;
            _event.Accessed = DateTime.Now;
            _event.NRepetitions = 1;
            db.Events.Add(_event);
            await db.SaveChangesAsync();
        }
        public async void Edit(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == tempLogin/*User.Identity.Name*/);
            if (id != null)
            {
                Event _event = await db.Events.Include(e => e.User).FirstOrDefaultAsync(p => p.Id == id);
                db.Events.Update(_event);
                await db.SaveChangesAsync();
            }
        }
        public async void Delete(int? id)
        {
            if (id != null)
            {
                Event _event = await db.Events.FirstOrDefaultAsync(p => p.Id == id);
                if (_event != null)
                {
                    db.Events.Remove(_event);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async void Mutatuion(Data.Event _event, int? projectIDforMutation)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == "defourtend"/*User.Identity.Name*/);
            _event = await db.Events.FirstOrDefaultAsync(t => _event.Id == t.Id);
            Data.Task task;
            if (projectIDforMutation != null)
                task = new Data.Task(_event, db.Projects.FirstOrDefault(p => p.Id == projectIDforMutation));
            else
                task = new Data.Task(_event, db.Projects.FirstOrDefault(p => p.Id != null));
            task.User = user;
            db.Tasks.Add(task);
            db.Events.Remove(_event);
            await db.SaveChangesAsync();
        }
    }
}
