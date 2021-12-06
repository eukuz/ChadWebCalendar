using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChadWebCalendar.Data
{
    public class Appointment: ComponentBase
    {
        [NameLengthValidator]
        public string Text { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
        public int RemindNMinutesBefore { get; set; }
        public int? Id { get; set; }
        public DateTime? Accessed { get; set; }

        [StartAndEndTimeValidator]
        public StartEndTimeModel StartEndTime { get; set; }

        public Appointment()
        {
            StartEndTime = new StartEndTimeModel();
        }
    }

    public class StartEndTimeModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}

