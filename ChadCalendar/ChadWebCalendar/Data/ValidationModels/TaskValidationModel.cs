using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChadWebCalendar.Data
{
    [Table("Tasks")]
    public class TaskValidation : Duty
    {
        public new int? Id { get; set; }
        public new DateTime? Accessed { get; set; }
        public new int? NRepetitions { get; set; } = 1;
        public new User User { get; set; }
        [Required]
        public bool IsCompleted { get; set; } = false; // false т.к при создании Task задача еще не выполнена
        [Required]
        public bool AllowedToDistribute { get; set; } = false; // избежание null
        [Required(ErrorMessage = "Поле не может быть пустым")]
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
    }
}

