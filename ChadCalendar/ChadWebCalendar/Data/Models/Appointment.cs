using System;

namespace ChadWebCalendar.Data
{
    public class Appointment
    {
        public string Text { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
        public int RemindNMinutesBefore { get; set; }

    }
}
