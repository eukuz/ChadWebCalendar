using System.ComponentModel.DataAnnotations;

namespace ChadCalendar.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password reqired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The start time of your working day reqired")]
        public int WorkingHoursFrom { get; set; }

        [Required(ErrorMessage = "The finish time of your working day reqired")]
        public int WorkingHoursTo { get; set; }

        [Required(ErrorMessage = "Your UTC reqired")]
        public int TimeZone { get; set; }
    }
}