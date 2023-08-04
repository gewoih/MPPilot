using System.Security.Cryptography;
using System.Text;

namespace MPBoom.Domain.Services.Security
{
    public static class PasswordHasher
    {
        public static string GetHashedString(string value, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(value);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            byte[] hashedBytes = SHA256.HashData(combinedBytes);

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
                stringBuilder.Append(hashedBytes[i].ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}
