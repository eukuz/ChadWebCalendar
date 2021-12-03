using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;


public class NameLengthValidator: ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            return new ValidationResult($"Поле <Название> не может быть пустым", new[] { validationContext.MemberName });
        if (value.ToString().Length > 40)
            return new ValidationResult($"Поле <Название> должно иметь меньше 40 символов", new[] { validationContext.MemberName });
        return null;
    }
}

