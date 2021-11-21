using BlazorChadCalendar.Data;
using BlazorChadCalendar.Infrastructure;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BlazorChadCalendar.Pages
{
    public class RegisterModel : ComponentBase
    {
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public CustomAuthStateProvider CustomAuthStateProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        public RegisterModel()
        {
            RegisterData = new RegisterViewModel();
        }
        public RegisterViewModel RegisterData { get; set; }
        protected async System.Threading.Tasks.Task RegisterAsync()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //Создаем пользователя в БД
                User user = new User
                {
                    Login = RegisterData.Login,
                    Password = RegisterData.Password,
                    WorkingHoursFrom = RegisterData.WorkingHoursFrom,
                    WorkingHoursTo = RegisterData.WorkingHoursTo,
                    TimeZone = RegisterData.Timezone,
                    RemindEveryNDays = 5
                };
                db.Users.Add(user);
                //Добавляем дефолтные данные
                Project project = new Project
                {
                    User = user,
                    Accessed = DateTime.Now,
                    Name = "Задачи",
                    IconNumber = "📝",
                    Description = "Базовый список задач"
                };
                db.Projects.Add(project);
                db.Tasks.Add(new Task { Name = "Создать задачу", TimeTakes = new TimeSpan(0, 5, 0), NRepetitions = 0, User = user, Project = project, Accessed = DateTime.Now });
                db.Tasks.Add(new Task { Name = "Создать событие", TimeTakes = new TimeSpan(0, 10, 0), NRepetitions = 0, User = user, Project = project, Accessed = DateTime.Now });
                db.SaveChanges();

                //Авторизация после регистрации
                var token = new SecurityToken
                {
                    AccessToken = RegisterData.Password,
                    UserName = RegisterData.Login,
                    ExpiredAt = DateTime.UtcNow.AddDays(3)
                };
                await LocalStorageService.SetAsync(nameof(SecurityToken), token);
                CustomAuthStateProvider.NotifyAuthenticationStateChanged();

                NavigationManager.NavigateTo("/", true);

            }
        }
    }


}
public class RegisterViewModel
{
    [Required]
    [UniqueLoginValidator]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }
    [Required]
    [Range(0, 24)]
    public int WorkingHoursFrom { get; set; }
    [Required]
    [Range(0, 24)]
    public int WorkingHoursTo { get; set; }
    [Required]
    [Range(-12, 14)]
    public int Timezone { get; set; }

}
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
