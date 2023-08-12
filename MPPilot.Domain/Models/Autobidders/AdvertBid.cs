using MPPilot.Domain.Models.Base;
using MPPilot.Domain.Utils;

namespace MPPilot.Domain.Models.Autobidders
{
	public class AdvertBid : Entity
	{
		public Guid AutobidderId { get; set; }
		public ChangeBidReason Reason { get; set; }
		public string ReasonString => Reason.GetDescription();
		public AutobidderMode AutobidderMode { get; set; }
		public string AdvertKeyword { get; set; }
		public int AdvertPosition { get; set; }
		public int TargetPositionLeftBound { get; set; }
		public int TargetPositionRightBound { get; set; }
		public int LastCPM { get; set; }
		public int CurrentCPM { get; set; }
		public int ChangeCPM => CurrentCPM - LastCPM;
	}
}
