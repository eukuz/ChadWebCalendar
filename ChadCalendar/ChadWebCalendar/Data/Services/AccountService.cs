using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChadWebCalendar.Models;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace ChadWebCalendar.Data.Services
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginModel model);
        Task<bool> LogoutAsync();
    }
    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _customAuthenticationProvider;
        private readonly ILocalStorageService _localStorageService;
        public AccountService(ILocalStorageService localStorageService,AuthenticationStateProvider customAuthenticationProvider)
        {
            _localStorageService = localStorageService;
            _customAuthenticationProvider = customAuthenticationProvider;
        }
        public async Task<bool> LoginAsync(LoginModel lModel)
        {

            //using (ApplicationContext db = new ApplicationContext())
            //{
            //    User user = db.Users.FirstOrDefault(u => u.Login == lModel.LoginData.model.Login && u.Password == lModel.LoginData.model.Password);
            //    if (user == null)
            //        return false;

                await _localStorageService.SetItemAsync("token", JsonSerializer.Serialize(lModel.LoginData.model));
                (_customAuthenticationProvider as CustomAuthenticationProvider).Notify();
                return true;
            //}  
           
        }

        public async Task<bool> LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("token");
            (_customAuthenticationProvider as CustomAuthenticationProvider).Notify();
            return true;
        }
    }
}
