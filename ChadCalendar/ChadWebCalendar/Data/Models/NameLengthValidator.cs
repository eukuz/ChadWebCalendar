using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class NameLengthValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return null;
        if (value.ToString().Length > 40)
            return new ValidationResult($"Название должно быть меньше 40 символов", new[] { validationContext.MemberName });
        return null;
    }
}

