using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadWebCalendar.Data
{
    [Table("Events")]
    public class Event : Duty
    {
        [Required]
        public DateTime StartsAt { get; set; }
        [Required]
        public DateTime FinishesAt { get; set; }
        public int RemindNMinutesBefore { get; set; }

        public Event(){} // Default constructor
        public Event(Task task, DateTime startsAt, int remindNMunutesBefore)
        {
            Accessed = DateTime.Now;
            Description = task.Description;
            StartsAt = startsAt;
            FinishesAt = (DateTime)(startsAt + (task.TimeTakes == null ? (new TimeSpan(0, 15, 0)) : task.TimeTakes));
            Frequency = task.Frequency;
            Name = task.Name;
            NRepetitions = task.NRepetitions;
            RemindNMinutesBefore = remindNMunutesBefore;
            User = task.User;
        }
    }
}

