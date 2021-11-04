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
        [Required]
        public decimal? HoursTakes { get; set; }
        public int? MaxPerDay { get; set; }
        public DateTime? Deadline { get; set; } = DateTime.Now.AddDays(1);
        public Task? Predecessor { get; set; }
        public Task? Successor { get; set; }
        [Required]
        public Project Project { get; set; }

        public bool IsCorrect()
        {
            if (NRepetitions < 0 || MaxPerDay < 0 || Deadline <= DateTime.Now || HoursTakes < 0 || Name == null || Deadline == null)
                return false;
            else
                return true;
        }
    }
}

