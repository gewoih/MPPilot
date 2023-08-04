namespace MPBoom.Domain.Services.Security.Token
{
    public interface ITokenService
    {
        public Task<bool> ValidateToken(string token);
        public Task SetTokenAsync(string token);
        public Task<string> GetTokenAsync();
        public Task RemoveTokenAsync();
    }
}
