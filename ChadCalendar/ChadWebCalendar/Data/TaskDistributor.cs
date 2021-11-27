
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadWebCalendar.Data
{
    public enum DistributionType
    {
        MaxInTheBeginning,
        //Evenly,
        //MaxInTheEnd
    }
    public static class TaskDistributor
    {
        public static void Distribute(Project project)
        {

            List<Data.Event> distributedTasks = new List<Data.Event>();
            using (ApplicationContext db = new ApplicationContext())
            {
                User u = db.Users.FirstOrDefault(u => u.Login == project.User.Login);
                
                DateTime endOfWeek = GetTheEndOfWorkingWeek(DayOfWeek.Saturday, u.WorkingHoursTo);

                List<Data.Event> eventsOfTheWeek = new List<Data.Event>(db.Events
                    .Where(e => e.User.Login == u.Login && e.FinishesAt > DateTime.Now && e.FinishesAt <= endOfWeek));
                
                AddNotWorkingHours(u.WorkingHoursFrom, u.WorkingHoursTo, eventsOfTheWeek, endOfWeek);
                eventsOfTheWeek = eventsOfTheWeek.OrderBy(e => e.StartsAt).ToList();

                List<TimeSlot> freeTimeSlots = findFreeTimeSlots(eventsOfTheWeek, endOfWeek);
                freeTimeSlots = freeTimeSlots.OrderBy(e => e.GetTimeSpan).ToList();

                List<Task> tasks = new List<Task>(db.Tasks.Where(t => t.Project.Id == project.Id && t.AllowedToDistribute));
                tasks = tasks.OrderByDescending(t => t.TimeTakes).ToList();

                bool[] pickedTasks = new bool[tasks.Count];

                for (int i = 0; i < freeTimeSlots.Count; i++)
                {
                    for (int j = 0; j < tasks.Count; j++)
                    {
                        if (!pickedTasks[j])
                        {
                            if (tasks[j].TimeTakes < freeTimeSlots[i].GetTimeSpan)
                            {
                                pickedTasks[j] = true;
                                distributedTasks.Add(new Event(tasks[j], freeTimeSlots[i].start, 15));
                                freeTimeSlots[i].start += (TimeSpan)tasks[j].TimeTakes;
                            }
                        }
                    }
                }
                db.Events.AddRange(distributedTasks);
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (pickedTasks[i])
                    {
                        db.Tasks.Remove(tasks[i]);
                    }
                }
                db.SaveChanges();
            }
        }
        private static List<TimeSlot> findFreeTimeSlots(List<Event> eventsOfTheWeek, DateTime endOfWeek)
        {
            List<TimeSlot> freeTimeSlots = new List<TimeSlot>();
            //if there none events
            if (eventsOfTheWeek.Count == 0) freeTimeSlots.Add(new TimeSlot(DateTime.Now, endOfWeek));
            else
            {
                //time before events started
                if (eventsOfTheWeek[0].StartsAt > DateTime.Now) freeTimeSlots.Add(new TimeSlot(DateTime.Now, eventsOfTheWeek[0].StartsAt)); 

                //time during events
                DateTime finishTakenTime = DateTime.Now;
                for (int i = 0; i < eventsOfTheWeek.Count - 1; i++)
                {
                    Event selectedEvent = eventsOfTheWeek[i];
                    if (finishTakenTime >= selectedEvent.FinishesAt) continue;//skip nested events

                    finishTakenTime = selectedEvent.FinishesAt;
                    Event nextEvent = eventsOfTheWeek[i + 1];

                    if (finishTakenTime < nextEvent.StartsAt) freeTimeSlots.Add(new(finishTakenTime, nextEvent.StartsAt));
                }
                //after events finished
                finishTakenTime = finishTakenTime < eventsOfTheWeek.Last().FinishesAt ? eventsOfTheWeek.Last().FinishesAt : finishTakenTime;
                if(finishTakenTime<endOfWeek) freeTimeSlots.Add(new(finishTakenTime, endOfWeek));
            }
            return freeTimeSlots;
        }

        private static void AddNotWorkingHours(int startWH, int finishWH, in List<Event> eventsOfTheWeek, DateTime endOfWeek)
        {
            DateTime startChill = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, finishWH, 0, 0);
            DateTime finishChill = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startWH, 0, 0);

            if (  finishWH > startWH )
                finishChill = finishChill.AddDays(1);
                

            for (int i = 0; i < (endOfWeek - DateTime.Now).TotalDays; i++)
            {
                eventsOfTheWeek.Add(new Event { Name = "Sleep", StartsAt = startChill, FinishesAt = finishChill });
                startChill = startChill.AddDays(1);
                finishChill = finishChill.AddDays(1);
            } 
        }
        private static int GetHourDifference(int finishWH, int startWH)
        {
            if (finishWH > startWH)
                startWH += 24;
            return startWH - finishWH;
        }

        private static DateTime GetTheEndOfWorkingWeek(DayOfWeek lastWorkingDayOfWeek, int workingHoursTo)
        {
            DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, workingHoursTo, 0, 0);
            int start = (int)from.DayOfWeek;
            int target = (int)lastWorkingDayOfWeek;

            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }



    }
    class TimeSlot
    {
        public DateTime start, finish;
        public TimeSpan GetTimeSpan { get => finish - start; }
        public TimeSlot(DateTime start, DateTime finish)
        {
            this.start = start;
            this.finish = finish;
        }
    }
}
