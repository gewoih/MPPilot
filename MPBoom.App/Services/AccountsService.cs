using Microsoft.EntityFrameworkCore;
using MPBoom.App.Infrastructure.Contexts;
using MPBoom.Domain.Exceptions;
using MPBoom.Domain.Models.Account;
using MPBoom.Domain.Services.Security;
using System.Security.Claims;

namespace MPBoom.App.Services
{
    public class AccountsService
    {
        private readonly MPBoomContext _context;

        public AccountsService(MPBoomContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(string name, string email, string password)
        {
            try
            {
                var newAccount = new Account
                {
                    Name = name,
                    Email = email,
                    Password = PasswordHasher.GetHashedString(password, email)
                };

                _context.Accounts.Add(newAccount);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException)
            {
                throw new UserAlreadyExistsException($"Пользователь с таким Email уже существует.");
            }
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(string email, string password)
        {
            var account = new Account
            {
                Email = email,
                Password = PasswordHasher.GetHashedString(password, email)
            };

            var findedAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == account.Email && a.Password == account.Password);
            if (findedAccount == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, account.Email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token");
            return claimsIdentity;
        }
    }
}
