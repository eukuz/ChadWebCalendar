using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChadWebCalendar.Data.Models
{
    public class LoginAuthValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            LoginViewModel model = value as LoginViewModel;
            using (ApplicationContext db = new ApplicationContext())
            {
                User u = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                if (u == null)
                    return new ValidationResult($"Пользователя с таким логином  и паролем не существует");
                return null;
            }
        }
    }
}

