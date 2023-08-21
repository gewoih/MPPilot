using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Accounts;
using MPPilot.Domain.Services.Token;
using MPPilot.Domain.Utils;
using System.Security.Claims;

namespace MPPilot.Domain.Services.Accounts
{
	public class AccountsService
	{
		private readonly MPPilotContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<AccountsService> _logger;
		private readonly ITokenService _tokenService;
		private Account _currentAccount;

		public AccountsService(MPPilotContext context, IHttpContextAccessor httpContextAccessor, ITokenService tokenService, ILogger<AccountsService> logger)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_tokenService = tokenService;
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

		public async Task<string> LoginAsync(string email, string password)
		{
			var account = await FindBycredentials(email, password);
			if (account is null)
			{
				_logger.LogInformation("Неудачная попытка входа в систему у пользователя {Email}", email);
				throw new ArgumentException("Пользователь с такими данными не найден в системе");
			}

			await CreateLoginHistory(account);
			_logger.LogInformation("Пользователь {Email} успешно вошел в систему.", email);

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
				new Claim(ClaimTypes.Email, account.Email),
				new Claim(ClaimTypes.Name, account.Name),
			};

			var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
			var token = _tokenService.GenerateToken(identity);

			return token;
		}

		private async Task CreateLoginHistory(Account account)
		{
			var userAgentString = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();

			var loginHistory = new LoginHistory
			{
				AccountId = account.Id,
				IPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
				DeviceName = UserAgentUtils.GetDevice(userAgentString),
				OSName = UserAgentUtils.GetOS(userAgentString),
				BrowserName = UserAgentUtils.GetBrowser(userAgentString),
				IsSuccessful = true
			};

			_context.LoginsHistory.Add(loginHistory);
			await _context.SaveChangesAsync();
		}

		public async Task SaveSettings(AccountSettings settings)
		{
			var currentAccount = GetCurrentAccount() ?? throw new UnauthorizedAccessException("Пользователь не авторизован");
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

		public AccountSettings? GetCurrentAccountSettings()
		{
			return _currentAccount?.Settings;
		}

		public async Task SetCurrentAccount(Guid accountId)
		{
			_currentAccount = await FindByIdAsync(accountId);
		}

		public Account GetCurrentAccount()
		{
			return _currentAccount;
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
						.AsNoTracking()
						.Include(account => account.Settings)
						.FirstOrDefaultAsync(account => account.Id.Equals(id));
		}
	}
}
