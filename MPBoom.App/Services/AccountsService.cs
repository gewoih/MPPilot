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

        public async Task<bool> RegisterAsync(Account account)
        {
            try
            {
                account.Password = PasswordHasher.GetHashedString(account.Password, account.Email);

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException)
            {
                throw new UserAlreadyExistsException($"Пользователь с таким Email уже существует.");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(Account account)
        {
            var passwordHash = PasswordHasher.GetHashedString(account.Password, account.Email);

            var findedAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == account.Email && a.Password == passwordHash);
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
