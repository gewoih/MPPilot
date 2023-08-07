using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Accounts;
using System.Security.Claims;

namespace MPPilot.Domain.Services
{
    public class AccountsService
    {
        private readonly MPPilotContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountsService(MPPilotContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task RegisterAsync(Account account)
        {
            try
            {
                account.Password = PasswordHasherService.GetHashedString(account.Password, account.Email);
                account.Settings = new();

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UserAlreadyExistsException($"Пользователь с таким Email уже существует.");
            }
        }

        public async Task<ClaimsIdentity> LoginAsync(string email, string password)
        {
            var account = await FindBycredentials(email, password) ?? throw new ArgumentException("Пользователь с такими данными не найден в системе");

			var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Name, account.Name),
            };

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            return identity;
        }

        public async Task SaveSettings(AccountSettings settings)
        {
            var currentAccount = await GetCurrentAccount() ?? throw new UnauthorizedAccessException("Пользователь не авторизован");
            currentAccount.Settings.WildberriesApiKey = settings.WildberriesApiKey;

            _context.Accounts.Update(currentAccount);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new APIKeyAlreadyExistsException("Данный API-ключ уже используется другим пользователем");
            }
        }

        public async Task<AccountSettings> GetCurrentAccountSettings()
        {
            var accountId = GetCurrentAccountId();
            var settings = await _context.AccountSettings
                                .Where(settings => settings.Account.Id == accountId)
                                .FirstAsync();

            return settings;
        }

        public async Task<Account?> GetCurrentAccount()
        {
            var accountId = GetCurrentAccountId();
            return await FindByIdAsync(accountId);
		}

		private Guid GetCurrentAccountId()
        {
			var currentUser = _httpContextAccessor?.HttpContext?.User;
			return currentUser is null
				? throw new UnauthorizedAccessException("Текущий пользователь не авторизован в системе")
				: Guid.Parse(currentUser.Claims.First(claim => claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase)).Value);
		}

        private async Task<Account?> FindBycredentials(string email, string password)
        {
            var passwordHash = PasswordHasherService.GetHashedString(password, email);

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email && a.Password == passwordHash);
            return account;
        }

        private async Task<Account?> FindByIdAsync(Guid id)
        {
            return await _context.Accounts
                        .Include(account => account.Settings)
                        .FirstOrDefaultAsync(account => account.Id.Equals(id));
        }
    }
}
