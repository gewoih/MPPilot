using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AccountsService> _logger;

        public AccountsService(MPPilotContext context, IHttpContextAccessor httpContextAccessor, ILogger<AccountsService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task RegisterAsync(Account account)
        {
            try
            {
                account.Password = PasswordHasherService.GetHashedString(account.Password, account.Email);
                account.Settings = new();

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Пользователь {Email} успешно зарегистрирован в системе.", account.Email);
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Неудачная попытка регистрации. Пользователь {Email} уже существует.", account.Email);
                throw new UserAlreadyExistsException($"Пользователь с таким Email уже существует.");
            }
        }

        public async Task<ClaimsIdentity> LoginAsync(string email, string password)
        {
            var account = await FindBycredentials(email, password);
            if (account is null)
            {
                _logger.LogInformation("Неудачная попытка входа в систему у пользователя {Email}", email);
                throw new ArgumentException("Пользователь с такими данными не найден в системе");
            }
            else
                _logger.LogInformation("Пользователь {Email} успешно вошел в систему.", email);

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
                _logger.LogInformation("Настройки аккаунта {Email} успешно сохранены.", currentAccount.Email);
            }
            catch (DbUpdateException)
            {
                _logger.LogWarning("Пользователь {Email} попытался сохранить API-ключ Wildberries ({ApiKey}) другого пользователя!", currentAccount.Email, settings.WildberriesApiKey);
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
            if (currentUser is null)
            {
                _logger.LogWarning("Неудачная попытка получения текущего пользователя");
                throw new UnauthorizedAccessException("Вы не авторизованы в системе. Пожалуйста, выполните повторный вход в аккаунт.");
            }
			
            return Guid.Parse(currentUser.Claims.First(claim => claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase)).Value);
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
