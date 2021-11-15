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

        public CustomAuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState CreateAnonymus()
            {
                var anonymusIdentity = new ClaimsIdentity();
                var anonymysPrincipal = new ClaimsPrincipal(anonymusIdentity);
                return new AuthenticationState(anonymysPrincipal);
            }

            
            var token = await _localStorageService.GetAsync<SecurityToken>(nameof(SecurityToken));


            if (token == null)
            {
                return CreateAnonymus();
            }
            if (string.IsNullOrEmpty(token.AccessToken) || token.ExpiredAt < System.DateTime.Now)
            {
                return CreateAnonymus();
            }

            //User user;
            //using (ApplicationContext db = new ApplicationContext())
            //{
            //    user = db.Users.FirstOrDefault(u => u.Login == token.UserName && u.Password == token.AccessToken);
            //}
            //if(user== null)
            //{
            //    return CreateAnonymus();
            //}

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name,token.UserName),
                new Claim(ClaimTypes.Role,"User"),
                new Claim(ClaimTypes.Expired,token.ExpiredAt.ToLongDateString())
            };


            var identity = new ClaimsIdentity(claims, "Token");
            var principal = new ClaimsPrincipal(identity);
            var state= new AuthenticationState(principal);
            return await System.Threading.Tasks.Task.FromResult(state);

        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}