using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadCalendar.Models
{
    [Table("Events")]
    public class Event : Duty
    {
        [Required]

        public DateTime StartsAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime FinishesAt { get; set; } = DateTime.Now.AddMinutes(10);
        public int RemindNMinutesBefore { get; set; }
        public bool IsCorrect()
        {
            if (!(StartsAt < FinishesAt) || RemindNMinutesBefore < 0 || Name == null)
                return false;
            else
                return true;
        }
    }
}

