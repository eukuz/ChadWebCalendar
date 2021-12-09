
using ChadWebCalendar.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadWebCalendar.Data
{
    public enum DistributionType
    {
        MaxInTheBeginning,
        Evenly,
        BestFit,
        MaxInTheEnd
    }
    public class DayHolder
    {
        public DateTime date;
        public TimeSpan freetime;

        public DayHolder(DateTime date, TimeSpan freetime)
        {
            this.date = date;
            this.freetime = freetime;
        }
    }
    public static class TaskDistributor
    {

        private static List<TimeSlot> SortForEvenly(List<TimeSlot> freeTimeSlots, DateTime endOfWeek, int workingHoursTo)//For now, assyming timeslot.start.Date = timeslot.finish.Date
        {

            int totalDays = (int)Math.Ceiling((endOfWeek - DateTime.Now).TotalDays);
            List<DayHolder> days = new List<DayHolder>();
            for (int i = 0 ; i < totalDays; i++)
                days.Add(new DayHolder(DateTime.Today.AddDays(i+(DateTime.Now.Hour < workingHoursTo ? 0 : 1)), new TimeSpan(0, 0, 0)));

            foreach (TimeSlot freeSlot in freeTimeSlots)
                days.FirstOrDefault(d => d.date == freeSlot.start.Date).freetime += freeSlot.GetTimeSpan;


            days = days.OrderByDescending(d => d.freetime).ToList();

            List<TimeSlot> newFreeTimeSlots = new List<TimeSlot>();

            foreach (DayHolder day in days)
                newFreeTimeSlots.AddRange(freeTimeSlots.Where(t => t.start.Date == day.date).OrderBy(t => t.GetTimeSpan));

            return newFreeTimeSlots;
        }
        public static void Distribute(int projId, DistributionType distributionType)
        {

            TaskService taskService = new TaskService();
            List<Data.Event> distributedTasks = new List<Data.Event>();
            using (ApplicationContext db = new ApplicationContext())
            {
                Project project = db.Projects.Include(p => p.User).FirstOrDefault(p => p.Id == projId);
                User user = project.User;

                DateTime endOfWeek = GetTheEndOfWorkingWeek(DayOfWeek.Saturday, user.WorkingHoursTo);

                List<Data.Event> eventsOfTheWeek = new List<Data.Event>(db.Events
                    .Where(e => e.User.Login == user.Login && e.FinishesAt > DateTime.Now && e.FinishesAt <= endOfWeek));

                AddNotWorkingHours(user.WorkingHoursFrom, user.WorkingHoursTo, eventsOfTheWeek, endOfWeek);
                eventsOfTheWeek = eventsOfTheWeek.OrderBy(e => e.StartsAt).ToList();

                List<TimeSlot> freeTimeSlots = findFreeTimeSlots(eventsOfTheWeek, endOfWeek);
                List<Task> tasks = new List<Task>(db.Tasks.Include(t=> t.Project).Where(t => t.Project.Id == project.Id && t.AllowedToDistribute && t.IsCompleted == false));
                tasks = tasks.OrderByDescending(t => t.TimeTakes).ToList();
                bool[] pickedTasks = new bool[tasks.Count];

                if (distributionType == DistributionType.Evenly)
                {
                    freeTimeSlots = SortForEvenly(freeTimeSlots, endOfWeek, user.WorkingHoursTo);

                    for (int j = 0; j < tasks.Count; j++)
                        for (int i = 0; i < freeTimeSlots.Count; i++)
                        {
                            if (!pickedTasks[j] && (tasks[j].TimeTakes <= freeTimeSlots[i].GetTimeSpan))
                            {
                                pickedTasks[j] = true;
                                distributedTasks.Add(new Event(tasks[j], freeTimeSlots[i].start, 15));
                                freeTimeSlots[i].start += (TimeSpan)tasks[j].TimeTakes;

                                freeTimeSlots = SortForEvenly(freeTimeSlots, endOfWeek, user.WorkingHoursTo);

                                break;
                            }
                        }
                }
                else
                {
                    if (distributionType == DistributionType.MaxInTheBeginning) freeTimeSlots = freeTimeSlots.OrderBy(e => e.start).ToList();
                    else if (distributionType == DistributionType.MaxInTheEnd) freeTimeSlots = freeTimeSlots.OrderByDescending(e => e.start).ToList();
                    else if (distributionType == DistributionType.BestFit) freeTimeSlots = freeTimeSlots.OrderBy(e => e.GetTimeSpan).ToList();

                    for (int i = 0; i < freeTimeSlots.Count; i++)
                        for (int j = 0; j < tasks.Count; j++)
                            if (!pickedTasks[j] && (tasks[j].TimeTakes <= freeTimeSlots[i].GetTimeSpan))
                            {
                                pickedTasks[j] = true;
                                distributedTasks.Add(new Event(tasks[j], freeTimeSlots[i].start, 15));
                                freeTimeSlots[i].start += (TimeSpan)tasks[j].TimeTakes;
                            }
                }


                db.Events.AddRange(distributedTasks);

                for (int i = 0; i < tasks.Count; i++)
                    if (pickedTasks[i]) {
                        var PredecessorDependecies = db.Tasks.Where(t => t.Predecessor == tasks[i]);
                        foreach (var item in PredecessorDependecies)
                        {
                            item.Predecessor = null;
                            db.Tasks.Update(item);
                        }
                        db.Tasks.Remove(tasks[i]);
                    }
                db.SaveChanges();
            }


        }
        private static void switchTimeSlots(in List<TimeSlot> freeTimeSlots, int i)
        {
            var t = freeTimeSlots[freeTimeSlots.Count - 1];
            freeTimeSlots[freeTimeSlots.Count - 1] = freeTimeSlots[i];
            freeTimeSlots[i] = t;
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
                if (finishTakenTime < endOfWeek) freeTimeSlots.Add(new(finishTakenTime, endOfWeek));
            }
            return freeTimeSlots;
        }

        private static void AddNotWorkingHours(int startWH, int finishWH, in List<Event> eventsOfTheWeek, DateTime endOfWeek)
        {
            DateTime startChill = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, finishWH, 0, 0);
            DateTime finishChill = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startWH, 0, 0);

            if (finishWH > startWH)
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
