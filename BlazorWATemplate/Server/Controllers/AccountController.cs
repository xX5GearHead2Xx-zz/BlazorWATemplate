using BlazorWATemplate.Shared.DBModels;
using BlazorWATemplate.Shared.DTOs.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BlazorWATemplate.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost, Route("RecoverPassword")]
        public async Task<ActionResult<ServiceResponse<bool>>> RecoverPassword(PasswordRecoveryDTO passwordRecovery)
        {
            var Result = await _accountService.RecoverPassword(passwordRecovery);
            return Ok(Result);
        }

        [HttpPost, Route("ResetPassword")]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var Result = await _accountService.ResetPassword(resetPassword);
            return Ok(Result);
        }

        [Authorize, HttpPost, Route("ChangePassword")]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword(ChangePasswordDTO changePassword)
        {
            string Email = HttpContext.User.Claims.Where(C => C.Type == ClaimTypes.Email).First().Value;
            var Result = await _accountService.ChangePassword(changePassword, Email);
            return Ok(Result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(RegisterDTO register)
        {
            if (await ValidateRecaptcha(register.RecaptchaResponse))
            {
                User user = new User()
                {
                    Name = register.Name,
                    Surname = register.Surname,
                    Email = register.Email
                };

                var response = await _accountService.Register(user, register.Password);
                return response;
            }
            else
            {
                return Unauthorized("Invalid Recaptcha");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginDTO login)
        {
            if (await ValidateRecaptcha(login.RecaptchaResponse))
            {
                var response = await _accountService.Login(login.Email, login.Password);
                return response;
            }
            else
            {
                return Unauthorized("Invalid Recaptcha");
            }
        }

        public async Task<bool> ValidateRecaptcha(string Captcha)
        {
            Uri RequestUri = new Uri("https://www.google.com/recaptcha/api/siteverify?secret=" + _configuration["Recaptcha:RecaptchaPrivate"] + "&response=" + Captcha);

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, RequestUri);

            var client = new HttpClient();
            var response = client.Send(httpRequest);

            RecaptchaDTO result = JsonConvert.DeserializeObject<RecaptchaDTO>(await response.Content.ReadAsStringAsync());

            return result.success;
        }
    }
}
