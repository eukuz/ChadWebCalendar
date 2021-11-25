using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace ChadWebCalendar.Data.Services
{
    public class NotificationsWorker
    {
        public static DateTime? FirstEventDT;
        public static bool IsStarted = false;
        public delegate void NotificationShowHandler();
        static public event NotificationShowHandler NotificationReadyToShow;
        Thread NotificationsCheckThread = new Thread(new ThreadStart(NotificationChecker));
        static ApplicationContext db = new ApplicationContext();
        static public Data.Event GetFirstEventByTime()
        {
            DateTime dt = DateTime.Now.AddMinutes(-5);
            var enumerable = db.Events.OrderBy(e => e != null);
            List<Data.Event> events = enumerable.ToList();
            List<Data.Event> eventsNew = new List<Event>();
            foreach (var item in events)
            {
                if (DateTime.Compare(item.StartsAt, dt) >= 0)
                {
                    eventsNew.Add(item);
                }
            }
            eventsNew.Add(null);
            return eventsNew[0];
        }
        public void Start()
        {
            NotificationsCheckThread.Start();
            IsStarted = true;
        }
        public void Stop()
        {
            NotificationsCheckThread.Abort();
            IsStarted = false;
        }
        private static void NotificationChecker()
        {
            while (true)
            {
                Debug.WriteLine("Worker is working");
                DateTime dt = DateTime.Now;
                Data.Event FirstEvent = GetFirstEventByTime();
                if (FirstEvent != null)
                    FirstEventDT = (FirstEvent.StartsAt.AddMinutes(-FirstEvent.RemindNMinutesBefore));
                if (FirstEventDT == null || dt >= FirstEventDT)
                {
                    Debug.WriteLine("Fuck yeah");
                    NotificationReadyToShow.Invoke();
                    if (FirstEvent != null)
                        FirstEventDT = (FirstEvent.StartsAt.AddMinutes(-FirstEvent.RemindNMinutesBefore)); // вычисление напоминалки
                    else
                        FirstEventDT = DateTime.Now.AddYears(1);
                }
                Thread.Sleep(5000);
            }
        }
    }
}
