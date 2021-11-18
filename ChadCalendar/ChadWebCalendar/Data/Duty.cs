using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadWebCalendar.Data
{

    [Table("Duties")]
    public abstract class Duty
    {
        [Required]
        public int? Id { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime? Accessed { get; set; }
        [Required(ErrorMessage = "Не указано NRepetitions")]
        public int? NRepetitions { get; set; } = 1;
        public string Frequency { get; set; }
        [Required]
        public User User { get; set; }
    }
}

