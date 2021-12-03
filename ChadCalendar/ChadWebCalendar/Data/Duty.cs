using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChadWebCalendar.Data
{

    [Table("Duties")]
    public abstract class Duty
    {
        public int Id { get; set; }

        [NameLengthValidator]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Accessed { get; set; }
        public int? NRepetitions { get; set; } = 1;
        public string Frequency { get; set; }
        public User User { get; set; }
    }
}

