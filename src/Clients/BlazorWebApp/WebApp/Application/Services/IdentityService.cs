using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models.User;
using WebApp.Extensions;
using WebApp.Utils;

namespace WebApp.Application.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly HttpClient httpClient;
        private readonly ISyncLocalStorageService syncLocalStorageService;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public IdentityService(HttpClient httpClient, ISyncLocalStorageService syncLocalStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.syncLocalStorageService = syncLocalStorageService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

        public string GetUserName()
        {
            return syncLocalStorageService.GetUsername();
        }

        public string GetUserToken()
        {
            return syncLocalStorageService.GetToken();
        }

        public async Task<bool> Login(string username, string password)
        {
            var request = new UserLoginRequest(username, password);

            var response = await httpClient.PostGetResponseAsync<UserLoginResponse, UserLoginRequest>("auth", request);

            if (!string.IsNullOrEmpty(response.UserToken))
            {
                syncLocalStorageService.SetToken(response.UserToken);
                syncLocalStorageService.SetUsername(response.UserName);

                ((AuthStateProvider)authenticationStateProvider).NotifyUserLogin(response.UserName);

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.UserToken);

                return true;
            }

            return false;

        }

        public void Logout()
        {
            syncLocalStorageService.RemoveItem("token");
            syncLocalStorageService.RemoveItem("username");

            ((AuthStateProvider)authenticationStateProvider).NotifyUserLogout();

            httpClient.DefaultRequestHeaders.Authorization = null;

        }
    }
}
