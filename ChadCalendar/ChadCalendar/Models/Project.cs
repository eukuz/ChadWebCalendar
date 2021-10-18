using System;
using System.ComponentModel.DataAnnotations;

namespace ChadCalendar.Models
{
    public class Project : Duty
    {
        public DateTime Deadline { get; set; }
        [Required]
        public int IconNumber { get; set; }
    }
}

