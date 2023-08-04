using MPBoom.Domain.Models.Token;
using MPBoom.Domain.Services.LocalStorage;

namespace MPBoom.Domain.Services.Token
{
    public class JWTTokenService : ITokenService
    {
        private const string _localStorageKey = "Token";
        private readonly ILocalStorageService _localStorageService;

        public JWTTokenService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SetTokenAsync(TokenDTO token)
        {
            await _localStorageService.SetItemAsync(_localStorageKey, token);
        }

        public async Task<TokenDTO> GetTokenAsync()
        {
            return await _localStorageService.GetItemAsync<TokenDTO>(_localStorageKey);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorageService.RemoveItemAsync(_localStorageKey);
        }
    }
}
