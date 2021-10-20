using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadCalendar.Models
{
    [Table("Tasks")]
    public class Task : Duty
    {
        public bool? IsCompleted { get; set; }
        public bool? AllowedToDistribute { get; set; }
        public decimal? HoursTakes { get; set; }
        public int? MaxPerDay { get; set; }
        public DateTime? Deadline { get; set; }
        public int? PredecessorFK { get; set; }
        public Task? Predecessor { get; set; }
        public int? SuccessorFK { get; set; }
        public Task? Successor { get; set; }
        [Required]
        public Project Project { get; set; }
    }
}

