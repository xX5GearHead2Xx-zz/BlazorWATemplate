using BlazorWATemplate.Client.Pages.Account;
using BlazorWATemplate.Shared.DTOs.Account;
using static System.Net.WebRequestMethods;

namespace BlazorWATemplate.Client.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _http;

        public AccountService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<string>> Login(LoginDTO login)
        {
            var result = await _http.PostAsJsonAsync("api/Account/login", login);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> Register(RegisterDTO register)
        {
            var result = await _http.PostAsJsonAsync("api/Account/register", register);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<bool>> ChangePassword(ChangePasswordDTO changePassword)
        {
            var response = await _http.PostAsJsonAsync($"api/Account/ChangePassword", changePassword);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> RecoverPassword(PasswordRecoveryDTO passwordRecovery)
        {
            var response = await _http.PostAsJsonAsync($"api/Account/RecoverPassword", passwordRecovery);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var response = await _http.PostAsJsonAsync($"api/Account/ResetPassword", resetPassword);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}
