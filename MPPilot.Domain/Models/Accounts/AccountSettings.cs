using MPPilot.Domain.Models.Base;

namespace MPPilot.Domain.Models.Accounts
{
	public class AccountSettings : Entity
	{
		public Account Account { get; set; }
		public string? WildberriesApiKey { get; set; }
	}
}
