using System;
using System.ComponentModel.DataAnnotations;

namespace ChadCalendar.Models
{
    public class Task : Duty
    {
        [Required]
        public bool IsCompleted { get; set; } = false;
        [Required]
        public decimal HoursTakes { get; set; } = 0;
        [Required]
        public bool AllowedToDistribute { get; set; } = false;
        public int MaxPerDay { get; set; }
        public DateTime Deadline { get; set; }
        public int PredecessorFK { get; set; } = 0;

        public int Discriminator { get; set; } = 0;
        public Task Predecessor { get; set; } = null;
        public int SuccessorFK { get; set; } = 0;
        public Task Successor { get; set; } = null;
        [Required]
        public Project Project { get; set; } = new Project();
    }
}

