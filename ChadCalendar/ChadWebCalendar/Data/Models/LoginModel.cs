using Blazored.LocalStorage;
using ChadWebCalendar.Data;
using ChadWebCalendar.Data.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChadWebCalendar.Models
{
    public class LoginModel : ComponentBase
    {
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public IAccountService _accountService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        public LoginModel()
        {
            LoginData = new LoginViewModelValidation { model = new LoginViewModel()};
        }
        protected async System.Threading.Tasks.Task LoginAsync()
        {
            await _accountService.LoginAsync(this);
            NavigationManager.NavigateTo("/", true);
        }
        public LoginViewModelValidation LoginData { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class LoginViewModelValidation
    {
        [Required]
        [LoginAuthValidator]
        public LoginViewModel model { get; set; }
    }
    public class LoginAuthValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            LoginViewModel model = value as LoginViewModel;
            using (ApplicationContext db = new ApplicationContext())
            {
                User u = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                if (u == null)
                    return new ValidationResult($"Пользователя с таким логином  и паролем не существует");
                return null;
            }
        }
    }
}

