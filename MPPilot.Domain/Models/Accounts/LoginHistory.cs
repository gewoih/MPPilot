using MPPilot.Domain.Models.Base;

namespace MPPilot.Domain.Models.Accounts
{
	public class LoginHistory : Entity
	{
		public Guid AccountId { get; set; }
		public bool IsSuccessful { get; set; }
		public string IPAddress { get; set; }
		public string DeviceName { get; set; }
		public string BrowserName { get; set; }
		public string OSName { get; set; }
	}
}
