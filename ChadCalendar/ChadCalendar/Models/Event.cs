using System;
using System.ComponentModel.DataAnnotations;

namespace ChadCalendar.Models
{
    public class Event : Duty
    {
        [Required]
        public DateTime StartsAt { get; set; }
        [Required]
        public DateTime FinishesAt { get; set; }
        public int RemindNMinutesBefore { get; set; }
    }
}

