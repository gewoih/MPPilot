using MPPilot.Domain.Models.Users;

namespace MPPilot.Domain.Services.Users
{
	public interface IUsersService
	{
		public Task ChangeSettingsAsync(UserSettings settings);
		public Task SetCurrentUserAsync(User account);
		public Task<User> GetCurrentUserAsync();
	}
}
