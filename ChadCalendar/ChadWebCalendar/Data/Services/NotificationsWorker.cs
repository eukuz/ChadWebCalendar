using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace ChadWebCalendar.Data.Services
{
    public class NotificationsWorker
    {
        public static string Username;
        public static DateTime? FirstEventDT;
        static bool WorkerIsWorking;
        public static DateTime? DeadlineNotification;
        public static string TypeOfNotification;
        public static bool DeadlinesIsInitializied = false;
        public static bool IsStarted = false;
        public delegate void NotificationShowHandler();
        static public event NotificationShowHandler NotificationReadyToShow;
        Thread NotificationsCheckThread = new Thread(new ThreadStart(NotificationChecker));
        static ApplicationContext db = new ApplicationContext();
        static public DateTime? GetFirstEventByTime()
        {
            DateTime? minDT = DateTime.Now.AddMinutes(35);
            DateTime dt = DateTime.Now.AddMinutes(-1);
            List<DateTime?> deadlines = new List<DateTime?>();

            var enumerableEvents = db.Events.OrderBy(e => e.StartsAt).Where(e => e.User.Login == Username);
            List<Data.Event> events = enumerableEvents.ToList();
            events.Add(null);
            if (events[0] != null)
            {
                if (!DeadlinesIsInitializied)
                {
                    events[0].StartsAt = events[0].StartsAt?.AddMinutes(-events[0].RemindNMinutesBefore);
                    if (events[0].StartsAt < minDT)
                    {
                        minDT = events[0].StartsAt;
                        TypeOfNotification = "event";
                        DeadlineNotification = events[0].StartsAt;
                    }
                }
                deadlines.Add(events[0].StartsAt);
            }

            var enumerableProjects = db.Projects.OrderBy(p => p.Deadline).Where(p => p.User.Login == Username);
            List<Data.Project> projectDeadlines = enumerableProjects.ToList();
            projectDeadlines.Add(null);
            if (projectDeadlines[0] != null)
            {
                if (!DeadlinesIsInitializied)
                    projectDeadlines[0].Deadline = projectDeadlines[0].Deadline; //?.AddMinutes(-30);
                if (projectDeadlines[0].Deadline < minDT)
                {
                    minDT = projectDeadlines[0].Deadline;
                    TypeOfNotification = "project";
                    DeadlineNotification = projectDeadlines[0].Deadline;
                }
                deadlines.Add(projectDeadlines[0].Deadline);
            }
            //при редактировании не редактируется тут, при добавлении нужно DeadlinesIsInitializied делать false
            var enumerableTasks = db.Tasks.OrderBy(t => t.Deadline).Where(t => t.User.Login == Username);
            List<Data.Task> taskDeadlines = enumerableTasks.ToList();
            taskDeadlines.Add(null);
            if (taskDeadlines[0] != null)
            {
                taskDeadlines[0].Deadline = taskDeadlines[0].Deadline;
                if (!DeadlinesIsInitializied)
                    taskDeadlines[0].Deadline = taskDeadlines[0].Deadline; //?.AddMinutes(-30);
                if (taskDeadlines[0].Deadline < minDT)
                {
                    minDT = taskDeadlines[0].Deadline;
                    TypeOfNotification = "task";
                    DeadlineNotification = taskDeadlines[0].Deadline;
                }
                deadlines.Add(taskDeadlines[0].Deadline);
            }

            //DeadlinesIsInitializied = true;

            var enumdeadlinesNew = deadlines.OrderBy(dt => dt);
            List<DateTime?> deadlinesNew = enumdeadlinesNew.ToList();
            List<DateTime?> deadlinesEnd = new List<DateTime?>();
            foreach (var item in deadlinesNew)
            {
                if (item != null && DateTime.Compare((DateTime) item, dt) >= 0)
                {
                    deadlinesEnd.Add(item);
                    break;
                }
            }
            deadlinesEnd.Add(null);
            return deadlinesEnd[0];
        }
        public void Start()
        {
            IsStarted = true;
            NotificationsCheckThread.Start();
        }
        public void Stop()
        {
            IsStarted = false;
        }
        private static void NotificationChecker()
        {
            while (IsStarted)
            {
                Debug.WriteLine("Worker is working");
                DateTime dt = DateTime.Now;
                DateTime? FirstEventDT = GetFirstEventByTime();
                if (dt >= FirstEventDT)
                {
                    Debug.WriteLine("Fuck yeah");
                    NotificationReadyToShow.Invoke();
                }
                Thread.Sleep(60000);
            }
        }
    }
}
