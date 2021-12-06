using ChadWebCalendar.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;


public class StartAndEndTimeValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        StartEndTimeModel model = value as StartEndTimeModel;
        if (model.Start >= model.End)
            return new ValidationResult($"Дата начала события не может быть позже даты окончания события");
        return null;
    }
}
