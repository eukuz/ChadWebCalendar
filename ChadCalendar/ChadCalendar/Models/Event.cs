using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadCalendar.Models
{
    [Table("Events")]
    public class Event : Duty
    {
        [Required]
        public DateTime? StartsAt { get; set; }
        [Required]
        public DateTime? FinishesAt { get; set; }
        public int RemindNMinutesBefore { get; set; }
        public bool IsCorrect()
        {
            if ((StartsAt >= FinishesAt) || RemindNMinutesBefore < 0 || Name == null || StartsAt == null || FinishesAt == null)
                return false;
            else
                return true;
        }
    }
}

