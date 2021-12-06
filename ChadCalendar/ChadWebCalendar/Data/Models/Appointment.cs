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

        public StartEndTimeModelValidation StartEndTime { get; set; }

        public Appointment()
        {
            StartEndTime = new StartEndTimeModelValidation { model = new StartEndTimeModel() };
        }
    }

    public class StartEndTimeModel
    {
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
    }
    public class StartEndTimeModelValidation
    {
        [Required]
        [StartAndEndTimeValidator]
        public StartEndTimeModel model { get; set; }
    }

}

