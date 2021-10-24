using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Models
{

    [Table("Duties")]
    public class Duty
    {
        //static public DateTime CutTheMilliseconds(DateTime inputDateTime)
        //{
        //    string tempDateTime = inputDateTime.ToString("MM:dd:yyyy");
        //    DateTime dateTime = DateTime.Parse(tempDateTime);
        //    return dateTime;
        //}
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Accessed { get; set; }
        public int? NRepetitions { get; set; }
        public int? Multiplier { get; set; }
        public string Frequency { get; set; }
        [Required]
        public User User { get; set; }
    }
}

