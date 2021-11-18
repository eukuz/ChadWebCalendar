using Blazored.LocalStorage;
using ChadWebCalendar.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChadWebCalendar.Data.Services
{
    public class CustomAuthenticationProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthenticationProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _localStorageService.GetItemAsync<string>("token");
            if (string.IsNullOrEmpty(token))
            {
                var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity() { }));
                return anonymous;
            }


            LoginViewModel login = JsonSerializer.Deserialize<LoginViewModel>(token);
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name,login.Login),
                new Claim(ClaimTypes.Role,"User"),
            };
            var identity = new ClaimsIdentity(claims, "token");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var loginUser = new AuthenticationState(claimsPrincipal);
            return loginUser;
        }

        public void Notify()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
