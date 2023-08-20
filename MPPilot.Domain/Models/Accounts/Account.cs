using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Models.Base;

namespace MPPilot.Domain.Models.Accounts
{
	public class Account : Entity
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public AccountSettings? Settings { get; set; }
		public Guid? AccountSettingsId { get; set; }

		public List<Autobidder> Autobidders { get; set; }

		public List<LoginHistory> LoginsHistory { get; set; }
	}
}
