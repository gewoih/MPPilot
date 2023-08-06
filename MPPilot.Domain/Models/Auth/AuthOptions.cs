using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace MPPilot.Domain.Models.Auth
{
    public class AuthOptions
    {
        private static string _key;
        public const string Issuer = "MPBoom.Server";
        public const string Audience = "MPBoom.Client";
        public static readonly TimeSpan Lifetime = TimeSpan.FromHours(2);
        public static SymmetricSecurityKey GetSecurityKey() => new(Encoding.UTF8.GetBytes(_key));

        public static void SetKey(IConfiguration configuration)
        {
            _key = configuration["JWT:Key"];
        }

        public static string GenerateRandomKey()
        {
            byte[] keyBytes = new byte[32];
            using var numberGenerator = RandomNumberGenerator.Create();
            numberGenerator.GetBytes(keyBytes);

            return Convert.ToBase64String(keyBytes);
        }
    }
}
