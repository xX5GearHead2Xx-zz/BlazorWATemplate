using BlazorWATemplate.Shared.DTOs.Account;
using BlazorWATemplate.Shared.DBModels;

namespace BlazorWATemplate.Server.Services.AccountService
{
    public interface IAccountService
    {
        Task<ServiceResponse<bool>> RecoverPassword(PasswordRecoveryDTO passwordRecovery);
        Task<ServiceResponse<bool>> ResetPassword(ResetPasswordDTO resetPassword);
        Task<ServiceResponse<bool>> ChangePassword(ChangePasswordDTO changePassword, string Email);
        Task<ServiceResponse<string>> Register(User user, string Password);
        Task<ServiceResponse<string>> Login(string Email, string Password);
        Task<ServiceResponse<bool>> ValidateRecaptcha(string recaptcha);
        Task<bool> UserExists(string Email);
        void HashString(string Input, out byte[] Hash, out byte[] Salt);
        bool VerifyHash(string Input, byte[] Hash, byte[] Salt);
    }
}
