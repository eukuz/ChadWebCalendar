using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class NoSpaceValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value.ToString().Any(Char.IsWhiteSpace)) 
            return new ValidationResult($"Избавьтесь от пробелов любых типов в данном поле", new[] { validationContext.MemberName });
        return null;
    }
}

