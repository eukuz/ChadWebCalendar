using Blazored.LocalStorage;
using ChadWebCalendar.Data;
using ChadWebCalendar.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChadWebCalendar.Data.Models
{
    public class UserSettingsModel : ComponentBase
    {
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public IAccountService _accountService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public string Login { get; set; }
        public UserSettingsModel()
        {

        }
        public UserSettingsViewModel SettingsData { get; set; } 
        protected async System.Threading.Tasks.Task RegisterAsync()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //Изменяем пользователя в БД
                User user = db.Users.FirstOrDefault(u => u.Login == SettingsData.Login);

                user.Login = SettingsData.Login;
                user.Password = SettingsData.Password;
                user.WorkingHoursFrom = SettingsData.workingHours.WorkingHoursFrom;
                user.WorkingHoursTo = SettingsData.workingHours.WorkingHoursTo;
                user.TimeZone = SettingsData.Timezone;
                user.RemindEveryNDays = SettingsData.RemindEveryNDays;

                db.Users.Update(user);
                await db.SaveChangesAsync();

                LoginModel login = new LoginModel();
                login.LoginData.model.Login = user.Login;
                login.LoginData.model.Password = user.Password;
                //Авторизация после изменения параметров
                await _accountService.LoginAsync(login);
                NavigationManager.NavigateTo("/", true);

            }
        }
    }


}
public class UserSettingsViewModel
{
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }
    public int Timezone { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int RemindEveryNDays { get; set; }

    [UserWorkingHoursValidator]
    public WorkingHoursModel WorkingHours { get; set; }

    public UserSettingsViewModel()
    {
        WorkingHours = new WorkingHoursModel();
    }
    public class WorkingHoursModel
    {
        [Required]
        [Range(0, 24)]
        public int WorkingHoursFrom { get; set; }
        [Required]
        [Range(0, 24)]
        public int WorkingHoursTo { get; set; }
    }
}

