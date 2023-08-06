using MPPilot.Domain.Models.Base;

namespace MPPilot.Domain.Models.Autobidders
{
	public class AdvertBid : Entity
	{
		public ChangeBidReason Reason { get; set; }
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
