using ChadWebCalendar.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static ChadWebCalendar.Data.Models.UserSettingsViewModel;
using static RegisterViewModel;

public class RegisterWorkingHoursValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        RegisterWorkingHoursModel model = value as RegisterWorkingHoursModel;
        if (model.WorkingHoursFrom >= model.WorkingHoursTo)
            return new ValidationResult($"Некорректная дата начала и конца рабочего дня");
        return null;
    }
}
