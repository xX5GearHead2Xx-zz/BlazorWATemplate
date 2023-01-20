using BlazorWATemplate.Shared.DTOs.Account;

namespace BlazorWATemplate.Client.Services.AccountService
{
    public interface IAccountService
    {
        Task<ServiceResponse<string>> Register(RegisterDTO register);
        Task<ServiceResponse<string>> Login(LoginDTO login);
        Task<ServiceResponse<bool>> RecoverPassword(PasswordRecoveryDTO passwordRecovery);
        Task<ServiceResponse<bool>> ResetPassword(ResetPasswordDTO resetPassword);
        Task<ServiceResponse<bool>> ChangePassword(ChangePasswordDTO changePassword);
    }
}
