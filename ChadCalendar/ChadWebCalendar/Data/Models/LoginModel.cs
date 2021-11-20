using Blazored.LocalStorage;
using ChadWebCalendar.Data.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace ChadWebCalendar.Data.Models
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
}

