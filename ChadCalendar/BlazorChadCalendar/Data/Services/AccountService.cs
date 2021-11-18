using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChadCalendar.Data.Services
{
    public interface IAccountService
    {
        bool Login();
        bool Logout();
    }
    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AccountService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }
        public bool Login()
        {
            (_authenticationStateProvider as CustomAuthStateProvider).LoginNotify();
            return true;
        }

        public bool Logout()
        {
            (_authenticationStateProvider as CustomAuthStateProvider).LogoutNotify();
            return true;
        }
    }
}
