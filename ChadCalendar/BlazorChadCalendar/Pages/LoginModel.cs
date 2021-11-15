using BlazorChadCalendar.Infrastructure;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChadCalendar.Pages
{
    public class LoginModel : ComponentBase
    {
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public CustomAuthStateProvider CustomAuthStateProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        public LoginModel()
        {
            LoginData = new LoginViewModel();
        }
        public LoginViewModel LoginData { get; set; }
        protected async Task LoginAsync()
        {
            var token = new SecurityToken
            {
                AccessToken = LoginData.Password,
                UserName = LoginData.Login,
                ExpiredAt = DateTime.UtcNow.AddDays(3)
            };
            await LocalStorageService.SetAsync(nameof(SecurityToken), token);
            CustomAuthStateProvider.NotifyAuthenticationStateChanged();
            NavigationManager.NavigateTo("/",true);
        }
    }
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

