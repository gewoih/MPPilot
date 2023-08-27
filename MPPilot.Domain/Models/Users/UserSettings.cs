using MPPilot.Domain.Models.Base;

namespace MPPilot.Domain.Models.Users
{
	public class UserSettings : Entity
	{
		public string? WildberriesApiKey { get; set; }
	}
}
