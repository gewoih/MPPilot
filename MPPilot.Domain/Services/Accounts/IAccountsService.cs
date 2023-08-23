using MPPilot.Domain.Models.Accounts;

namespace MPPilot.Domain.Services.Accounts
{
	public interface IAccountsService
	{
		public Task RegisterAsync(UserCredentials credentials);
		public Task<string> LoginAsync(UserCredentials credentials);
		public Task ChangeSettingsAsync(AccountSettings settings);
		public Task<AccountSettings> GetSettingsAsync();
		public Task SetCurrentAccountAsync(Guid id);
		public Task<Account> GetCurrentAccountAsync();
	}
}
