using Microsoft.IdentityModel.Tokens;
using MPBoom.Domain.Models.Token;
using MPBoom.Domain.Services.LocalStorage;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MPBoom.Domain.Services.Security.Token
{
    public class JWTTokenService : ITokenService
    {
        private const string _localStorageKey = "Token";
        private readonly ILocalStorageService _localStorageService;

        public JWTTokenService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorageService.SetItemAsync(_localStorageKey, token);
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>(_localStorageKey);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorageService.RemoveItemAsync(_localStorageKey);
        }

        public async Task<bool> ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AuthOptions.Issuer,
                ValidAudience = AuthOptions.Audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = AuthOptions.GetSecurityKey()
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                return true;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }
    }
}
