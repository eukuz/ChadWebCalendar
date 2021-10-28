using System.ComponentModel.DataAnnotations;

namespace ChadCalendar.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
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
        public bool RemindMe { get; set; } = true;
        public int RemindEveryNDays { get; set; }


    }
}