using System.ComponentModel;

namespace MPPilot.Domain.Models.Autobidders
{
	public enum ChangeBidReason
	{
		[Description("За границами средней ставки")]
		NotAverageCpm
	}
}