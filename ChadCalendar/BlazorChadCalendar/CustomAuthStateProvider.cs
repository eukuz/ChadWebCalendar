using BlazorChadCalendar.Data;
using BlazorChadCalendar.Infrastructure;
using BlazorChadCalendar.Pages;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorChadCalendar
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state= new AuthenticationState(claimsPrincipal);
            return await System.Threading.Tasks.Task.FromResult(state);
        }

        public async void LoginNotify()
        {
            ClaimsPrincipal CreateAnonymus() => new(new ClaimsIdentity());

            var token = await _localStorageService.GetAsync<SecurityToken>(nameof(SecurityToken));


            if (token == null)
            {
                claimsPrincipal =  CreateAnonymus();
            }
            if (string.IsNullOrEmpty(token.AccessToken) || token.ExpiredAt < System.DateTime.Now)
            {
                claimsPrincipal = CreateAnonymus();
            }


            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name,token.UserName),
                new Claim(ClaimTypes.Role,"User"),
                new Claim(ClaimTypes.Expired,token.ExpiredAt.ToLongDateString())
            };


            var identity = new ClaimsIdentity(claims, "Token");
            claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void LogoutNotify()
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity()); 
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}