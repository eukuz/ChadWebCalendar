using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChadWebCalendar.Data.Services
{
    public class EventService
    {
        static ApplicationContext db = new ApplicationContext();

        bool IsCorrect(ref Data.Event _event)
        {
            if (_event.Name != null && _event.Name != "")
                return true;
            else
                return false;
        }
        public int? GetId(string Name)
        {
            Data.User user = db.Users.FirstOrDefault(e => e.Login == Name);
            return user.Id;
        }
        public Data.User GetUser(string Name)
        {
            return db.Users.FirstOrDefault(u => u.Login == Name);
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
        public bool Create(Event _event, string Name)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == Name);
            _event.User = user;
            _event.Accessed = DateTime.Now;
            _event.NRepetitions = 1;
            if (IsCorrect(ref _event))
            {
                db.Events.Add(_event);
                db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public bool Edit(int? id, string Name)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == Name);
            Event _event = db.Events.Include(e => e.User).FirstOrDefault(p => p.Id == id);
            if (IsCorrect(ref _event))
            {
                db.Events.Update(_event);
                db.SaveChanges();
                return true;
            }
            return false;
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
        public async void Mutatuion(Data.Event _event, int? projectIDforMutation, string Name)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == Name);
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
