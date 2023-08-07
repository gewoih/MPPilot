using MPPilot.Domain.Models.Accounts;
using MPPilot.Domain.Models.Base;

namespace MPPilot.Domain.Models.Autobidders
{
	public class Autobidder : Entity
	{
		public bool IsActive { get; set; }
		public int AdvertId { get; set; }
		public AutobidderMode Mode { get; set; }
		public Account Account { get; set; }
		public List<AdvertBid> Bids { get; set; }
	}
}
