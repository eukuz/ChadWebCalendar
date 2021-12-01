using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChadWebCalendar.Data
{
    [Table("Tasks")]
    public class Task : Duty
    {
        public bool IsCompleted { get; set; } = false; // false т.к при создании Task задача еще не выполнена
        public bool AllowedToDistribute { get; set; } = true; // избежание null
        public TimeSpan? TimeTakes { get; set; }
        public int? MaxPerDay { get; set; }
        public DateTime? Deadline { get; set; }
        public int? PredecessorFK { get; set; }
        [ForeignKey("PredecessorFK")]
        public Task Predecessor { get; set; }
        public Project Project { get; set; }

        public bool IsCorrect()
        {
            if (NRepetitions < 0 || MaxPerDay < 0 || Name == null || TimeTakes == null || Name == "")
                return false;
            else
                return true;
        }
        public Task() { }
        public Task(Event _event, Project project)
        {
            Accessed = DateTime.Now;
            Description = _event.Description;
            AllowedToDistribute = true;
            Deadline = _event.FinishesAt;
            TimeTakes = _event.FinishesAt - _event.StartsAt;
            IsCompleted = false;
            MaxPerDay = 0;
            Predecessor = null;
            Project = project;
            Frequency = _event.Frequency;
            Name = _event.Name;
            NRepetitions = _event.NRepetitions;
            User = _event.User;
        }
    }
}

