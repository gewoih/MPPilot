using MPPilot.Domain.Models.Base;
using MPPilot.Domain.Models.Users;

namespace MPPilot.Domain.Models.Autobidders
{
	public class Autobidder : Entity
	{
		public bool IsEnabled { get; set; }
		public int AdvertId { get; set; }
		public AutobidderMode Mode { get; set; }
		public double DailyBudget { get; set; }
		public DateTimeOffset? BidsPausedTill { get; set; }
		public User User { get; set; }
		public List<AdvertBid> Bids { get; set; }
	}
}
