using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MPBoom.Domain.Exceptions;
using MPBoom.Domain.Models.Account;
using MPBoom.Domain.Services.Security;
using MPBoom.Domain.Services.Security.Token;
using MPPilot.App.Infrastructure;
using MPPilot.App.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MPPilot.App.Services
{
	public class AccountsService
    {
        private readonly MPBoomContext _context;
        private readonly ITokenService _tokenService;

        public AccountsService(MPBoomContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> RegisterAsync(AccountDTO accountDTO)
        {
            try
            {
                var account = new Account
                {
                    Name = accountDTO.Name,
                    Email = accountDTO.Email,
                    Password = PasswordHasher.GetHashedString(accountDTO.Password, accountDTO.Email)
                };

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

        public async Task<ClaimsIdentity> LoginAsync(AccountDTO accountDTO)
        {
            var account = await GetAccount(accountDTO);
            if (account is null)
                return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, account.Email),
				new Claim(ClaimTypes.Name, account.Name)
			};

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            return identity;
        }

        public async Task<bool> SaveSettings(AccountDTO accountDTO, SettingsDTO settings)
        {
            var account = await GetAccount(accountDTO);
            account.AccountSettings = new AccountSettings
            {
                WildberriesApiKey = settings.WildberriesApiKey
            };

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<Account> GetAccount(AccountDTO accountDTO)
        {
            var passwordHash = PasswordHasher.GetHashedString(accountDTO.Password, accountDTO.Email);

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == accountDTO.Email && a.Password == passwordHash);
            return account;
        }
    }
}
