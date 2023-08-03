using Microsoft.EntityFrameworkCore;
using MPBoom.API.Infrastructure.Contexts;
using MPBoom.Domain.Exceptions;
using MPBoom.Domain.Models.Account;
using MPBoom.Domain.Services;

namespace MPBoom.API.Services
{
    public class AccountsService
    {
        private readonly MPBoomContext _context;

        public AccountsService(MPBoomContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(string name, string email, string password)
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

        public async Task<bool> Login(string email, string password)
        {
            var account = new Account
            {
                Email = email,
                Password = PasswordHasher.GetHashedString(password, email)
            };

            var isFinded = await _context.Accounts.AnyAsync(a => a.Email == account.Email && a.Password == account.Password);
            return isFinded;
        }
    }
}
