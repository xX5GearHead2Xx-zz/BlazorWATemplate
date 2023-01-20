using BlazorWATemplate.Server.Azure;
using BlazorWATemplate.Server.MailGenerator;
using BlazorWATemplate.Shared.DTOs.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using BlazorWATemplate.Shared.DBModels;

namespace BlazorWATemplate.Server.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly ISecurityService _securityService;
        private readonly BlazorTemplateContext _Context;
        private readonly IConfiguration _configuration;
        public AccountService(BlazorTemplateContext Context, IConfiguration configuration, ISecurityService securityService)
        {
            _securityService = securityService;
            _Context = Context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(ChangePasswordDTO changePassword, string Email)
        {
            ServiceResponse<bool> Response = new ServiceResponse<bool>();

            if (await UserExists(Email))
            {
                User user = await _Context.Users.FirstAsync(u => u.Email.ToLower() == Email.ToLower());

                if (VerifyHash(changePassword.OldPassword, user.Password, user.Salt))
                {
                    HashString(changePassword.NewPassword, out byte[] Hash, out byte[] Salt);
                    user.Password = Hash;
                    user.Salt = Salt;
                    await _Context.SaveChangesAsync();

                    Response.Message = "Password has been updated successfully";
                    Response.Success = false;
                }
                else
                {
                    Response.Message = "Incorrect old password supplied";
                    Response.Success = false;
                }
            }
            else
            {
                Response.Message = "The email entered is not registered";
                Response.Success = false;
            }

            return Response;
        }

        public async Task<ServiceResponse<bool>> RecoverPassword(PasswordRecoveryDTO passwordRecovery)
        {
            ServiceResponse<bool> Response = new();

            if (await UserExists(passwordRecovery.Email))
            {
                User user = await _Context.Users.FirstAsync(u => u.Email.ToLower() == passwordRecovery.Email.ToLower());

                //Create the password recovery
                PasswordRecovery Recovery = new();
                Recovery.UserId = user.UserId;
                Recovery.Date = DateTime.UtcNow;
                Recovery.Expiry = DateTime.UtcNow.AddHours(12);
                _Context.PasswordRecoveries.Add(Recovery);
                await _Context.SaveChangesAsync();

                //Build the password recovery email
                TemplateBuilder Template = new(Enums.EmailTemplate.PasswordRecovery);
                string EncryptedRecoveryID = await _securityService.EncryptAsync(Recovery.PasswordRecoveryId.ToString());
                string RecoveryLink = _configuration["SiteInfo:URL"].ToString() + "/Account/ResetPassword?RecoveryID=" + EncryptedRecoveryID;
                Template.ReplacePlaceholder("{RecoveryLink}", RecoveryLink);

                //Send the recovery email
                Communication CommsHandler = new(_configuration);
                CommsHandler.SendEmail("Password Recovery", Template.Body.ToString(), new List<string> { passwordRecovery.Email });

                Response.Message = "Please check your email (" + passwordRecovery.Email + ") for instructions on how to reset your password";
            }
            else
            {
                Response.Message = "The email entered is not registered";
                Response.Success = false;
            }

            return Response;
        }

        public async Task<ServiceResponse<bool>> ResetPassword(ResetPasswordDTO resetPassword)
        {
            ServiceResponse<bool> Response = new();

            string decryptedRecoveryID = await _securityService.DecryptAsync(resetPassword.PasswordRecoveryID);

            if (int.TryParse(decryptedRecoveryID, out int RecoveryID))
            {
                PasswordRecovery Recovery = _Context.PasswordRecoveries.FirstAsync(R => R.PasswordRecoveryId == RecoveryID).Result;

                if (Recovery.Expiry > DateTime.UtcNow)
                {
                    User User = await _Context.Users.FirstAsync(U => U.UserId == Recovery.UserId);
                    HashString(resetPassword.Password, out byte[] Hash, out byte[] Salt);
                    User.Password = Hash;
                    User.Salt = Salt;

                    //Set the recovery to expired as the user has successfully changed their password
                    Recovery.Expiry = DateTime.UtcNow;

                    await _Context.SaveChangesAsync();

                    Response.Message = "Your password has been changed successfully, please log in using your new password.";
                }
                else
                {
                    Response.Success = false;
                    Response.Message = "That password recovery link has expired, please request another password recovery link.";
                }
            }

            return Response;
        }

        public class RecaptchaObject
        {
            public bool success { get; set; } = false;
        }

        public async Task<ServiceResponse<bool>> ValidateRecaptcha(string Captcha)
        {
            ServiceResponse<bool> serviceResponse = new();
            try
            {
                Uri RequestUri = new Uri("https://www.google.com/recaptcha/api/siteverify?secret=" + _configuration["Recaptcha:RecaptchaPrivate"] + "&response=" + Captcha);

                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, RequestUri);

                var client = new HttpClient();
                var response = client.Send(httpRequest);

                RecaptchaObject result = JsonConvert.DeserializeObject<RecaptchaObject>(await response.Content.ReadAsStringAsync());

                serviceResponse.Data = true;
            }
            catch
            {
                serviceResponse.Data = false;
                serviceResponse.Success = false;
                serviceResponse.Message = "Could not verify recaptcha";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> Register(User User, string Password)
        {
            var response = new ServiceResponse<string>();
            if (await UserExists(User.Email))
            {
                response.Success = false;
                response.Message = "Email is already registered";
            }
            else
            {
                HashString(Password, out byte[] Hash, out byte[] Salt);

                User.Password = Hash;
                User.Salt = Salt;
                User.CreateDate = DateTime.UtcNow;
                User.LastActive = DateTime.UtcNow;
                _Context.Users.Add(User);
                await _Context.SaveChangesAsync();

                response.Data = CreateToken(User.UserId);
                response.Message = "Account created successfully";
            }
            return response;
        }

        public async Task<ServiceResponse<string>> Login(string Email, string Password)
        {
            var response = new ServiceResponse<string>();
            var user = await _Context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == Email.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User does not exist";
            }
            else if (VerifyHash(Password, user.Password, user.Salt))
            {
                response.Message = "Welcome back " + user.Email;
                response.Data = CreateToken(user.UserId);
            }
            else
            {
                response.Success = false;
                response.Message = "Incorrect password.";
            }
            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _Context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());
        }

        public void HashString(string Input, out byte[] Hash, out byte[] Salt)
        {
            using (var hmac = new HMACSHA512())
            {
                Salt = hmac.Key;
                Hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Input));
            }
        }

        public bool VerifyHash(string Input, byte[] Hash, byte[] Salt)
        {
            using (var hmac = new HMACSHA512(Salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Input));
                return computedHash.SequenceEqual(Hash);
            }
        }

        private string CreateToken(int UserID)
        {
            string jwt = string.Empty;

            var user = _Context.Users
                .Include(role => role.LinkUserRoles)
                    .ThenInclude(role => role.AccessType)
                .Include(role => role.LinkUserRoles)
                    .ThenInclude(role => role.Role)
                .SingleOrDefault(u => u.UserId == UserID);

            if (user != null)
            {
                List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name, new Guid().ToString() + ":" + user.Name + " " + user.Surname)
            };

                foreach (LinkUserRole UserRole in user.LinkUserRoles)
                {
                    Enums.AccessType AccessType = (Enums.AccessType)UserRole.AccessType.AccessTypeId;
                    Enums.UserRole Role = (Enums.UserRole)UserRole.Role.RoleId;

                    //When combined, will make a description like ProductManagerEdit or DeveloperReadOnly etc.
                    string RoleDescription = Role.ToString() + AccessType.ToString();
                    claims.Add(new Claim(ClaimTypes.Role, RoleDescription));
                }

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AuthenticationSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                    );

                jwt = new JwtSecurityTokenHandler().WriteToken(token);
            }

            return jwt;

        }
    }
}
