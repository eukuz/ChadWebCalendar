using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChadCalendar.Models
{
    [Table("Tasks")]
    public class Task : Duty
    {
       
        [Required]
        public bool? IsCompleted { get; set; } = false; // false т.к при создании Task задача еще не выполнена
        [Required]
        public bool AllowedToDistribute { get; set; } = false; // избежание null
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public double? HoursTakes { get; set; }
        public int? MaxPerDay { get; set; }
        public DateTime? Deadline { get; set; } = DateTime.Now.AddDays(1);
        public Task? Predecessor { get; set; }
        [Required]
        public Project Project { get; set; }

        public bool IsCorrect()
        {
            if (NRepetitions < 0 || MaxPerDay < 0 || HoursTakes < 0 || Name == null || HoursTakes == null || Name == "")
                return false;
            else
                return true;
        }
        public Task() { }
        public Task(Event _event, Project project)
        {
            Accessed = DateTime.Now;
            Description = this.Description;
            AllowedToDistribute = true;
            Deadline = _event.FinishesAt;
            HoursTakes = (_event.FinishesAt - _event.StartsAt).Value.TotalHours;
            IsCompleted = false;
            MaxPerDay = 1;
            Predecessor = null;
            Project = project;
            Frequency = _event.Frequency;
            Name = _event.Name;
            NRepetitions = _event.NRepetitions;
            User = _event.User;
        }
    }
}

