using TimeTracker.Shared.Models.Account;
using System.Net.Http.Json;
using Blazored.Toast.Services;
using TimeTracker.Shared.Models.Login;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace TimeTracker.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IToastService _toastService;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;   

        public AuthService(
            HttpClient httpClient, 
            IToastService toastService, 
            NavigationManager navigationManager, 
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _toastService = toastService;
            _navigationManager = navigationManager;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task Login(LoginRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/login", request);
            if(result != null)
            {
                var response = await result.Content.ReadFromJsonAsync<LoginResponse>();
                if(!response.IsSuccessful && response.Error != null) {
                    _toastService.ShowError(response.Error);
                }
                else if(!response.IsSuccessful) {
                    _toastService.ShowError("An unexpected error occurred.");
                }
                else { 
                    if(response.Token != null) {
                        await _localStorage.SetItemAsStringAsync("authToken", response.Token);
                        await _authStateProvider.GetAuthenticationStateAsync();  
                    }
                    // _toastService.ShowSuccess("Login successful");
                    _navigationManager.NavigateTo("timeentries");
                }
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _authStateProvider.GetAuthenticationStateAsync(); // at this point auth state provider wont get an auth token
            _navigationManager.NavigateTo("login");
        }

        public async Task Register(AccountRegistrationRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account", request);
            if (result != null) {
                var response = await result.Content.ReadFromJsonAsync<AccountRegistrationResponse>();
                if (!response.IsSuccessful && response.Errors != null)
                {
                    foreach (var error in response.Errors)
                    {
                        _toastService.ShowError(error);
                    }
                }
                else if(!response.IsSuccessful) {
                    _toastService.ShowError("An unexpected error occurred.");
                }
                else { 
                    _toastService.ShowSuccess("Registration successful. You may now login.");
                }
            }
        }
    }
}