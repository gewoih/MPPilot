using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MPBoom.Domain.Models.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MPBoom.Domain.Services.Security.Token
{
	public class JWTTokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JWTTokenService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(ClaimsIdentity identity)
        {
            var securityKey = AuthOptions.GetSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AuthOptions.Issuer,
                Audience = AuthOptions.Audience,
                Subject = identity,
                Expires = DateTime.UtcNow.Add(AuthOptions.Lifetime),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

		public string GetToken()
		{
			var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is not null)
            {
                var token = httpContext.Request.Cookies[JwtBearerDefaults.AuthenticationScheme];
                return token;
            }

            return null;
		}

		public bool ValidateToken(string token)
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
