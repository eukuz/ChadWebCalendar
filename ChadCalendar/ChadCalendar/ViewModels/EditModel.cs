using System.ComponentModel.DataAnnotations;

namespace ChadCalendar.ViewModels
{
    public class EditModel
    {
        [Required(ErrorMessage = "ID required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Login required")]
        public string Login { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The start time of your working day required")]
        [Range(0, 24)]
        public int WorkingHoursFrom { get; set; }

        [Required(ErrorMessage = "The finish time of your working day required")]
        [Range(0, 24)]
        public int WorkingHoursTo { get; set; }

        [Required(ErrorMessage = "Your UTC reqired")]
        [Range(-12, 14)]
        public int TimeZone { get; set; }
        public bool RemindMe { get; set; }
        public int RemindEveryNDays { get; set; }


    }
}