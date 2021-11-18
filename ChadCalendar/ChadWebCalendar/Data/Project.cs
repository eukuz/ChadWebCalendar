using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadWebCalendar.Data
{
    [Table("Projects")]
    public class Project : Duty
    {
        public DateTime? Deadline { get; set; }
        public string IconNumber { get; set; }
    }
}

