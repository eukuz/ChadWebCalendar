using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Models
{

    [Table("Duties")]
    public abstract class Duty
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime? Accessed { get; set; }
        [Required]
        public int? NRepetitions { get; set; } = 1;
        public string Frequency { get; set; }
        [Required]
        public User User { get; set; }
    }
}

