using Microsoft.AspNetCore.Identity;
using MPPilot.Domain.Models.Autobidders;

namespace MPPilot.Domain.Models.Users
{
	public class User : IdentityUser<Guid>
	{
		public UserSettings? Settings { get; set; }
		public Guid? UserSettingsId { get; set; }

		public List<Autobidder> Autobidders { get; set; }

		public List<LoginHistory> LoginsHistory { get; set; }
	}
}
