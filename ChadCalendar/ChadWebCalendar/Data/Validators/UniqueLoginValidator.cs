using ChadWebCalendar.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class UniqueLoginValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            User u = db.Users.FirstOrDefault(u => u.Login == value.ToString());
            if (u == null)
                return null;
            return new ValidationResult($"Пользователь с таким логином уже существует, выберите другой", new[] { validationContext.MemberName });
        }
    }
}

