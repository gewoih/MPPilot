using System.Security.Claims;

namespace MPPilot.Domain.Services.Security.Token
{
    public interface ITokenService
    {
        public string GetToken();
        public string GenerateToken(ClaimsIdentity identity);
        public bool ValidateToken(string token);
    }
}
