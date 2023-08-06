using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MPPilot.App.Models;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Account;
using MPPilot.Domain.Services.Security;
using MPPilot.Domain.Services.Security.Token;
using System.Security.Claims;

namespace MPPilot.Domain.Services.Account
{
    public class AccountsService
    {
        private readonly MPPilotContext _context;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountsService(MPPilotContext context, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
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
            var account = await FindBycredentials(accountDTO);
            if (account is null)
                return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Name, account.Name),
            };

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            return identity;
        }

        public async Task<bool> SaveSettings(AccountSettingsDTO settings)
        {
            try
            {
                var currentAccount = await GetCurrentAccount();
                currentAccount.AccountSettings = new AccountSettings
                {
                    WildberriesApiKey = settings.WildberriesApiKey
                };

                _context.Accounts.Update(currentAccount);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Account?> GetCurrentAccount()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            if (currentUser is null)
                return null;

            var userId = Guid.Parse(currentUser.Claims.First(claim => claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase)).Value);

            return await FindByIdAsync(userId);
        }

        private async Task<Account?> FindBycredentials(AccountDTO accountDTO)
        {
            var passwordHash = PasswordHasher.GetHashedString(accountDTO.Password, accountDTO.Email);

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == accountDTO.Email && a.Password == passwordHash);
            return account;
        }

        private async Task<Account?> FindByIdAsync(Guid id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(account => account.Id.Equals(id));
        }
    }
}
