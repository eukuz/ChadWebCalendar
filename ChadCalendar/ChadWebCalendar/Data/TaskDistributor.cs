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
                    .Where(e => e.User.Login == u.Login && e.FinishesAt > DateTime.Now && e.FinishesAt <= endOfWeek)
                    .OrderBy(e => e.StartsAt);





            }
        }
        private static List<(DateTime, DateTime)> findFreeTimeSlots(List<Event> eventsOfTheWeek, DateTime endOfWeek, int startWH, int finishWH)
        {
            List<(DateTime, DateTime)> freeTimeSlots = new List<(DateTime, DateTime)>();
            //if there none events
            if (eventsOfTheWeek.Count == 0) freeTimeSlots.Add(new(DateTime.Now, endOfWeek));
            else
            {
                //time before events started
                if (eventsOfTheWeek[0].StartsAt > DateTime.Now) freeTimeSlots.Add(new(DateTime.Now, eventsOfTheWeek[0].StartsAt)); 

                //time during events
                DateTime finishTakenTime = DateTime.Now;
                for (int i = 0; i < eventsOfTheWeek.Count - 1; i++)
                {
                    Event selectedEvent = eventsOfTheWeek[i];
                    if (finishTakenTime >= selectedEvent.FinishesAt) continue;//skip nested events

                    finishTakenTime = selectedEvent.FinishesAt;
                    Event nextEvent = eventsOfTheWeek[i + 1];


                    if (finishTakenTime < nextEvent.StartsAt) {
                        DateTime startTakenTime = nextEvent.StartsAt;
                        if (finishTakenTime.Hour > finishWH && startTakenTime.Hour < startWH) continue;
                        if (finishTakenTime.Hour > finishWH) finishTakenTime.AddHours(GetHourDifference(finishWH, startWH));
                        else
                        {
                            if (startTakenTime.Hour < startWH) startTakenTime.AddHours(GetHourDifference(startTakenTime.Hour,startWH));
                            //TODO finish the method
                        }
                        freeTimeSlots.Add(new(finishTakenTime, startTakenTime));
                    };
                }
                //after events finished
                finishTakenTime = finishTakenTime < eventsOfTheWeek.Last().FinishesAt ? eventsOfTheWeek.Last().FinishesAt : finishTakenTime;
                if(finishTakenTime<endOfWeek) freeTimeSlots.Add(new(finishTakenTime, endOfWeek));
            }
            return freeTimeSlots;

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
}
