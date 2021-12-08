using ChadWebCalendar.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static ChadWebCalendar.Data.Models.UserSettingsViewModel;

public class UserWorkingHoursValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        WorkingHoursModel model = value as WorkingHoursModel;
        if (model.WorkingHoursFrom >= model.WorkingHoursTo)
            return new ValidationResult($"Некорректная дата начала и конца рабочего дня");
        return null;
    }
}
