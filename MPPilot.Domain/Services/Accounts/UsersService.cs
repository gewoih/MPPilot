using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Users;
using MPPilot.Domain.Utils;

namespace MPPilot.Domain.Services.Accounts
{
	public class UsersService : IUsersService
	{
		private readonly MPPilotContext _context;
		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<UsersService> _logger;
		private User _currentUser;

		public UsersService(MPPilotContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ILogger<UsersService> logger)
		{
			_context = context;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_logger = logger;
		}

		public async Task ChangeSettingsAsync(UserSettings settings)
		{
			_ = _currentUser ?? throw new UnauthorizedAccessException("Пользователь не авторизован");
			_currentUser.Settings = new UserSettings
			{
				WildberriesApiKey = settings.WildberriesApiKey
			};

			try
			{
				await _userManager.UpdateAsync(_currentUser);
				_logger.LogInformation("Настройки аккаунта {Email} успешно сохранены.", _currentUser.Email);
			}
			catch (DbUpdateException)
			{
				_logger.LogWarning("Пользователь {Email} попытался сохранить API-ключ Wildberries ({ApiKey}) другого пользователя!", _currentUser.Email, settings.WildberriesApiKey);
				throw new APIKeyAlreadyExistsException("Данный API-ключ уже используется другим пользователем");
			}
		}

		public async Task SetCurrentUserAsync(User account)
		{
			_currentUser = account;
		}

		public async Task<User> GetCurrentUserAsync()
		{
			return _currentUser;
		}

		private async Task CreateLoginHistory(User account)
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
	}
}
