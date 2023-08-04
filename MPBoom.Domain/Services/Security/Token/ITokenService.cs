using MPBoom.Domain.Models.Token;

namespace MPBoom.Domain.Services.Security.Token
{
    public interface ITokenService
    {
        public Task SetTokenAsync(TokenDTO token);
        public Task<TokenDTO> GetTokenAsync();
        public Task RemoveTokenAsync();
    }
}
