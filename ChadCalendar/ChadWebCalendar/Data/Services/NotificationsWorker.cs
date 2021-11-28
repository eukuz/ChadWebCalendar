using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;


namespace ChadWebCalendar.Data.Services
{
    public class NotificationsWorker
    {
        public static string Username;
        public static DateTime? FirstEventDT;
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
            DateTime? minDT = DateTime.Now.AddMinutes(Constants.AdditionInMinutesForMinDT);
            DateTime dt = DateTime.Now.AddMinutes(Constants.MinutesBeforeForInterval);
            List<DateTime?> deadlines = new List<DateTime?>();

            var enumerableEvents = db.Events.AsNoTracking().OrderBy(e => e.StartsAt).Where(e => e.User.Login == Username && e.StartsAt >= dt);
            List<Data.Event> events = enumerableEvents.ToList();
            events.Add(null);
            if (events[0] != null)
            {
                if (!DeadlinesIsInitializied)
                {
                    events[0].StartsAt = events[0].StartsAt.AddMinutes(-events[0].RemindNMinutesBefore);
                    if (events[0].StartsAt < minDT)
                    {
                        minDT = events[0].StartsAt;
                        string stripped;
                        if (minDT.ToString().Count() == Constants.CountOfSymbolsForFullDateTime)
                            stripped = minDT.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForFullDT);
                        else
                            stripped = minDT.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForTrimmedDT);
                        TypeOfNotification = $"Ваше событие {events[0].Name} начинается в {stripped}";
                        DeadlineNotification = events[0].StartsAt;
                    }
                }
                deadlines.Add(events[0].StartsAt);
            }

            var enumerableProjects = db.Projects.AsNoTracking().OrderBy(p => p.Deadline).Where(p => p.User.Login == Username && p.Deadline >= dt);
            List<Data.Project> projectDeadlines = enumerableProjects.ToList();
            projectDeadlines.Add(null);
            string tempOfNotification = "";
            if (projectDeadlines[0] != null && projectDeadlines[0].Deadline - dt > (dt - dt))
            {
                string stripped;
                if (projectDeadlines[0].ToString().Count() == Constants.CountOfSymbolsForFullDateTime)
                    stripped = projectDeadlines[0].ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForFullDT);
                else
                    stripped = projectDeadlines[0].ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForTrimmedDT);
                if ((projectDeadlines[0].Deadline - dt) < (dt.AddMinutes(60) - dt))
                {
                    projectDeadlines[0].Deadline = projectDeadlines[0].Deadline?.AddMinutes(-30);
                    tempOfNotification = $"Дедлайн проекта {projectDeadlines[0].Name} в {stripped}";
                }
                else if ((projectDeadlines[0].Deadline - dt) < (dt.AddHours(3) - dt))
                {
                    projectDeadlines[0].Deadline = projectDeadlines[0].Deadline?.AddHours(-2);
                    tempOfNotification = $"Дедлайн проекта {projectDeadlines[0].Name} в {stripped}";
                }
                else if ((projectDeadlines[0].Deadline - dt) < (dt.AddDays(2) - dt))
                {
                    projectDeadlines[0].Deadline = projectDeadlines[0].Deadline?.AddDays(-1);
                    tempOfNotification = $"Дедлайн проекта {projectDeadlines[0].Name} уже завтра в {stripped}";
                }
                else if ((projectDeadlines[0].Deadline - dt) < (dt.AddDays(8) - dt))
                {
                    projectDeadlines[0].Deadline = projectDeadlines[0].Deadline?.AddDays(-7);
                    tempOfNotification = $"Дедлайн проекта {projectDeadlines[0].Name} через неделю";
                }
                else if ((projectDeadlines[0].Deadline - dt) < (dt.AddDays(32) - dt))
                {
                    projectDeadlines[0].Deadline = projectDeadlines[0].Deadline?.AddDays(-30);
                    tempOfNotification = $"Дедлайн проекта {projectDeadlines[0].Name} через 30 дней";
                }
                if (projectDeadlines[0].Deadline < minDT && projectDeadlines[0].Deadline >= dt)
                {
                    minDT = projectDeadlines[0].Deadline;
                    TypeOfNotification = tempOfNotification;
                    DeadlineNotification = projectDeadlines[0].Deadline;
                }
                deadlines.Add(projectDeadlines[0].Deadline);
            }
            //при редактировании не редактируется тут, при добавлении нужно DeadlinesIsInitializied делать false
            var enumerableTasks = db.Tasks.AsNoTracking().OrderBy(t => t.Deadline).Where(t => t.User.Login == Username && t.Deadline >= dt);
            List<Data.Task> taskDeadlines = enumerableTasks.ToList();
            taskDeadlines.Add(null);
            tempOfNotification = "";
            if (taskDeadlines[0] != null && taskDeadlines[0].Deadline - dt > (dt - dt))
            {
                string stripped;
                if (taskDeadlines[0].ToString().Count() == Constants.CountOfSymbolsForFullDateTime)
                    stripped = taskDeadlines[0].ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForFullDT);
                else
                    stripped = taskDeadlines[0].ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForTrimmedDT);
                if ((taskDeadlines[0].Deadline - dt) < (dt.AddMinutes(60) - dt))
                {
                    taskDeadlines[0].Deadline = taskDeadlines[0].Deadline?.AddMinutes(-30);
                    tempOfNotification = $"Дедлайн задачи {taskDeadlines[0].Name} в {stripped}";
                }
                else if ((taskDeadlines[0].Deadline - dt) < (dt.AddHours(3) - dt))
                {
                    taskDeadlines[0].Deadline = taskDeadlines[0].Deadline?.AddHours(-2);
                    tempOfNotification = $"Дедлайн задачи {taskDeadlines[0].Name} в {stripped}";
                }
                else if ((taskDeadlines[0].Deadline - dt) < (dt.AddDays(2) - dt))
                {
                    taskDeadlines[0].Deadline = taskDeadlines[0].Deadline?.AddDays(-1);
                    tempOfNotification = $"Дедлайн задачи {taskDeadlines[0].Name} завтра в {stripped}";
                }
                else if ((taskDeadlines[0].Deadline - dt) < (dt.AddDays(8) - dt))
                {
                    taskDeadlines[0].Deadline = taskDeadlines[0].Deadline?.AddDays(-7);
                    tempOfNotification = $"Дедлайн задачи {taskDeadlines[0].Name} через неделю";
                }
                else if ((taskDeadlines[0].Deadline - dt) < (dt.AddDays(32) - dt))
                {
                    taskDeadlines[0].Deadline = taskDeadlines[0].Deadline?.AddDays(-30);
                    tempOfNotification = $"Дедлайн задачи {taskDeadlines[0].Name} через 30 дней";
                }
                if (taskDeadlines[0].Deadline < minDT && taskDeadlines[0].Deadline >= dt)
                {
                    minDT = taskDeadlines[0].Deadline;
                    TypeOfNotification = tempOfNotification;
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
                db = new ApplicationContext();
                Debug.WriteLine("Worker is working");
                DateTime dt = DateTime.Now;
                DateTime? FirstEventDT = GetFirstEventByTime();
                if (dt >= FirstEventDT)
                {
                    Debug.WriteLine("Fuck yeah");
                    NotificationReadyToShow.Invoke();
                }
                Thread.Sleep(Constants.MillisecondsForSleepAfterWorkerIteration);
            }
        }
    }
}
