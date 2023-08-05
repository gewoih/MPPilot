using System.Security.Claims;

namespace MPBoom.Domain.Services.Security.Token
{
	public interface ITokenService
	{
		public string GenerateToken(ClaimsIdentity identity);
		public bool ValidateToken(string token);
	}
}
