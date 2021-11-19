using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadWebCalendar.Data
{
    [Table("Events")]
    public class EventValidation : Duty
    {
        public new int? Id { get; set; }
        public new DateTime? Accessed { get; set; }
        public new User User { get; set; }
        public DateTime? StartsAt { get; set; }
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

