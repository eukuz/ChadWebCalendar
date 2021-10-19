using System;
using System.ComponentModel.DataAnnotations;

namespace ChadCalendar.Models
{
    public class Task : Duty
    {
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public decimal HoursTakes { get; set; }
        [Required]
        public bool AllowedToDistribute { get; set; }
        public int MaxPerDay { get; set; }
        public DateTime Deadline { get; set; }
        public int PredecessorFK { get; set; }

        public int Discriminator { get; set; }
        public Task Predecessor { get; set; }
        public int SuccessorFK { get; set; }
        public Task Successor { get; set; }
        [Required]
        public Project Project { get; set; }
    }
}

