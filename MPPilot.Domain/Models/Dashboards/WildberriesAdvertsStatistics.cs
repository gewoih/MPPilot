using MPPilot.Domain.Services.Marketplaces;

namespace MPPilot.Domain.Models.Dashboards
{
	public class WildberriesAdvertsStatistics
	{
		public double TotalBudgetInAdverts { get; set; }
		public double TotalBudgetInAutobidders { get; set; }
		public int ActiveAutobiddersCount { get; set; }
		public int ActiveAdvertsCount { get; set; }
		public WildberriesBalance WildberriesBalance { get; set; }
	}
}
