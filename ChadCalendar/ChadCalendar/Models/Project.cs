using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadCalendar.Models
{
    [Table("Projects")]
    public class Project : Duty
    {
        public DateTime? Deadline { get; set; }
        public string IconNumber { get; set; }
    }
}

