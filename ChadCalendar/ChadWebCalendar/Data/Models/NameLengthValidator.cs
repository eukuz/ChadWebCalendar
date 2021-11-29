using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;


public class NameLengthValidator: ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value.ToString().Length > 40)
            return new ValidationResult($"Слишком длинное название", new[] { validationContext.MemberName });
        return null;
    }
}

