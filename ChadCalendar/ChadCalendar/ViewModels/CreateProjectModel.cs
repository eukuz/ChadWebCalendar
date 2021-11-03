using ChadCalendar.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace ChadCalendar.ViewModels
{
    public class CreateProjectModel
    {
        [Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Не указано NRepetitions")]
        public int? NRepetitions { get; set; }
        public string Frequency { get; set; }
        public DateTime? Deadline { get; set; }
        public string IconNumber { get; set; }
        public User User { get; internal set; }
        public DateTime Accessed { get; internal set; }
    }
}
