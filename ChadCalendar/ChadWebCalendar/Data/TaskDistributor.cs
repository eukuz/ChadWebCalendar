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
        MaxInTheEnd
    }
    public static class TaskDistributor
    {
        public static void Distribute(Project project)
        {

            List<Event> distributedTasks = new List<Event>();
            using (ApplicationContext db = new ApplicationContext())
            {
                User u = db.Users.FirstOrDefault(u => u.Login == project.User.Login);
                DateTime endOfWeek = GetTheEndOfWorkingWeek(DayOfWeek.Saturday, u.WorkingHoursTo);

                List<Event> eventsOfTheWeek = (List<Event>)db.Events
                    .Where(e => e.User.Login == u.Login && e.FinishesAt > DateTime.Now && e.FinishesAt <= endOfWeek);
                
                AddNotWorkingHours(u.WorkingHoursFrom, u.WorkingHoursTo, eventsOfTheWeek, endOfWeek);
                eventsOfTheWeek.OrderBy(e => e.StartsAt);

                List<TimeSlot> freeTimeSlots = findFreeTimeSlots(eventsOfTheWeek,endOfWeek);
                freeTimeSlots.OrderByDescending(e => e.GetTimeSpan);

                List<Task> tasks = new List<Task>(db.Tasks.Where(t => t.Project.Id == project.Id));
                tasks.OrderByDescending(t => t.TimeTakes);

                bool noneApplropraiteSlots = freeTimeSlots.Count > 0;
                int i = 0, j = 0;
                while (!noneApplropraiteSlots && tasks.Count > 0)
                {
                    if ( freeTimeSlots[i].GetTimeSpan > tasks[j].TimeTakes)
                    {
                        distributedTasks.Add(new Event(tasks[j], freeTimeSlots[i].start, 10));
                        freeTimeSlots[i].start += (TimeSpan)tasks[j].TimeTakes;
                        if (freeTimeSlots[i].GetTimeSpan.TotalMinutes == 0) freeTimeSlots.RemoveAt(i);
                    }
                }
                //for (int i = 0; i < freeTimeSlots.Count; i++)
                //{
                //    for (int j = 0; j < tasks.Count; j++)
                //    {
                //        if (!pickedTasks[j])
                //        {
                //            if (tasks[j].TimeTakes < freeTimeSlots[i].GetTimeSpan)
                //            {
                //                pickedTasks[j] = true;
                //                distributedTasks.Add(new Event(tasks[j], freeTimeSlots[i].Item1, 10));
                //                freeTimeSlots[i].Item1 += tasks[j].TimeTakes;
                //            }
                //        }
                //    }
                //}

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
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, finishWH, 0, 0);
            DateTime finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startWH, 0, 0);

            if (  finishWH > startWH )
                finish.AddDays(1);
                

            for (int i = 0; i < (endOfWeek - DateTime.Now).TotalDays; i++)
            {
                eventsOfTheWeek.Add(new Event { Name = "Sleep", StartsAt = start, FinishesAt = finish });
                start.AddDays(1);
                finish.AddDays(1);
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
            DateTime from = DateTime.Now;
            int start = (int)from.DayOfWeek;
            int target = (int)lastWorkingDayOfWeek;

            if (target <= start)
                target += 7;
            from.AddDays(target - start);

            return new DateTime(from.Year, from.Month, from.Day, workingHoursTo, 0, 0);
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
