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
        public int WorkingHoursFrom { get; set; }

        [Required(ErrorMessage = "The finish time of your working day required")]
        public int WorkingHoursTo { get; set; }

        [Required(ErrorMessage = "Your UTC reqired")]
        public int TimeZone { get; set; }
        //[Required(ErrorMessage = "Should we remind you about forgotten duties?")]
        public bool RemindMe { get; set; }
        //[Required(ErrorMessage = "Required the number of days. With this frequency we'll remind you about duties you don't interact with (considering them forgotten)")]
        public int RemindEveryNDays { get; set; }


    }
}