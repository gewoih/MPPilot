using Microsoft.IdentityModel.Tokens;
using MPBoom.Domain.Models.Token;
using System.IdentityModel.Tokens.Jwt;

namespace MPBoom.Domain.Services.Security.Token
{
    public class JWTTokenService : ITokenService
    {
        private const string _localStorageKey = "Token";

        public Task<string> GetTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task SetTokenAsync(string token)
        {
            throw new NotImplementedException();
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
