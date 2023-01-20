namespace BlazorWATemplate.Server.Services.SecurityService
{
    public interface ISecurityService
    {
        public Task<string> EncryptAsync(string Input);
        public Task<string> DecryptAsync(string Input);
    }
}
