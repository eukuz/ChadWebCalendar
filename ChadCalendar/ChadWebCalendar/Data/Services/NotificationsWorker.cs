using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace ChadWebCalendar.Data.Services
{
    public class NotificationsWorker
    {
        BackgroundWorker NotificationsCheckThread = new BackgroundWorker();
        public List<string> TypesOfNotification = new List<string>();
        static ApplicationContext db = new ApplicationContext();

        public delegate void NotificationShowHandler();
        public event NotificationShowHandler NotificationReadyToShow;

        public static string Username;
        public bool isFirstStart = true;
        public bool IsStarted = false;

        public void WorkerInitialization(string Login)
        {
            if (!IsStarted && Login != null)
            {
                Username = Login;
                Start();
                IsStarted = true;
            }
        }
        public void GetReadyNotifications()
        {
            TypesOfNotification = new List<string>();
            DateTime dt = DateTime.Now.AddMinutes(Constants.MinutesBeforeForInterval);
            removeSecondsAndMilliseconds(ref dt);
            var enumerableEvents = db.Events.AsNoTracking().OrderBy(e => e.StartsAt).Where(e => e.User.Login == Username && e.StartsAt >= dt);
            List<Data.Event> events = enumerableEvents.ToList();
            foreach (var item in events)
            {
                if (item != null)
                {
                    DateTime TimeToNotificate = item.StartsAt.AddMinutes(-(item.RemindNMinutesBefore));
                    if (TimeToNotificate == dt)
                    {
                        string stripped;
                        if (TimeToNotificate.ToString().Count() == Constants.CountOfSymbolsForFullDateTime)
                        {
                            stripped = item.StartsAt.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForFullDT);
                        }
                        else if (TimeToNotificate.ToString().Last() == 'm')
                        {
                            stripped = item.StartsAt.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForFullDT + 2);
                        }
                        else
                        {
                            stripped = item.StartsAt.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForTrimmedDT);
                        }
                        TypesOfNotification.Add($"Ваше событие {item.Name} начинается в {stripped}");
                    }
                }
            }
            
            var enumerableProjects = db.Projects.AsNoTracking().OrderBy(p => p.Deadline).Where(p => p.User.Login == Username && p.Deadline >= dt);
            List<Data.Project> projectDeadlines = enumerableProjects.ToList();
            string tempOfNotification = "";
            foreach (var item in projectDeadlines)
            {
                if (item != null && item.Deadline - dt >= (dt - dt))
                {
                    string stripped;
                    if (item.ToString().Count() == Constants.CountOfSymbolsForFullDateTime)
                        stripped = item.Deadline.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForFullDT);
                    else
                        stripped = item.Deadline.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForTrimmedDT);
                    if ((item.Deadline - dt) == (dt.AddMinutes(30) - dt))
                    {
                        Debug.WriteLine(item.Deadline - dt);
                        tempOfNotification = $"Дедлайн проекта {item.Name} в {stripped}";
                    }
                    else if ((item.Deadline - dt) == (dt.AddHours(2) - dt))
                    {
                        tempOfNotification = $"Дедлайн проекта {item.Name} в {stripped}";
                    }
                    else if ((item.Deadline - dt) == (dt.AddDays(1) - dt))
                    {
                        tempOfNotification = $"Дедлайн проекта {item.Name} уже завтра в {stripped}";
                    }
                    else if ((item.Deadline - dt) == (dt.AddDays(7) - dt))
                    {
                        tempOfNotification = $"Дедлайн проекта {item.Name} через неделю";
                    }
                    else if ((item.Deadline - dt) == (dt.AddDays(30) - dt))
                    {
                        tempOfNotification = $"Дедлайн проекта {item.Name} через 30 дней";
                    }
                    if (tempOfNotification != String.Empty)
                    {
                        TypesOfNotification.Add(tempOfNotification);
                    }
                }
            }

            var enumerableTasks = db.Tasks.AsNoTracking().OrderBy(t => t.Deadline).Where(t => t.User.Login == Username && t.Deadline >= dt);
            List<Data.Task> taskDeadlines = enumerableTasks.ToList();
            tempOfNotification = "";
            foreach (var item in taskDeadlines)
            {
                if (!item.IsCompleted && item != null && item.Deadline - dt > (dt - dt))
                {
                    string stripped;
                    if (item.ToString().Count() == Constants.CountOfSymbolsForFullDateTime)
                        stripped = item.Deadline.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForFullDT);
                    else
                        stripped = item.Deadline.ToString().Substring(Constants.NumberOfInitialTimePosition, Constants.LenForTrimmedDT);
                    if ((item.Deadline - dt) == (dt.AddMinutes(30) - dt))
                    {
                        tempOfNotification = $"Дедлайн задачи {item.Name} в {stripped}";
                    }
                    else if ((item.Deadline - dt) == (dt.AddHours(2) - dt))
                    {
                        tempOfNotification = $"Дедлайн задачи {item.Name} в {stripped}";
                    }
                    else if ((item.Deadline - dt) == (dt.AddDays(1) - dt))
                    {
                        tempOfNotification = $"Дедлайн задачи {item.Name} завтра в {stripped}";
                    }
                    else if ((item.Deadline - dt) == (dt.AddDays(7) - dt))
                    {
                        tempOfNotification = $"Дедлайн задачи {item.Name} через неделю";
                    }
                    else if ((item.Deadline - dt) == (dt.AddDays(30) - dt))
                    {
                        tempOfNotification = $"Дедлайн задачи {item.Name} через 30 дней";
                    }
                    if (tempOfNotification != String.Empty)
                    {
                        TypesOfNotification.Add(tempOfNotification);
                    }
                }
            }
        }
        public void Start()
        {
            if (isFirstStart)
            {
                NotificationsCheckThread.DoWork += NotificationChecker;
                isFirstStart = false;
            }

            IsStarted = true;
            NotificationsCheckThread.RunWorkerAsync();
        }
        public void Stop()
        {
            IsStarted = false;
            Thread.Sleep(500);
        }
        private void NotificationChecker(object sender, DoWorkEventArgs e)
        {
            while (IsStarted)
            {
                db = new ApplicationContext();
                Debug.WriteLine("Worker is working");
                DateTime dt = DateTime.Now;
                GetReadyNotifications();
                if (TypesOfNotification.Count != 0)
                {
                    Debug.WriteLine("NotificationsShow");
                    NotificationReadyToShow.Invoke();
                }
                TypesOfNotification.Clear();
                Thread.Sleep(Constants.MillisecondsForSleepAfterWorkerIteration);
            }
        }
        private void removeSecondsAndMilliseconds(ref DateTime dateTime)
        {
            dateTime = dateTime.Date.AddHours(dateTime.Hour).AddMinutes(dateTime.Minute);
        }
    }
}
