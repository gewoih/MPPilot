namespace MPPilot.Domain.Models.Adverts
{
	public class AdvertMarketStatistics
	{
		public List<AdvertMarketInfo> MarketAdverts { get; set; }
		public int AdvertPosition { get; set; }
		public int Subject { get; set; }
		public double AverageCPM => MarketAdverts.Any() ? MarketAdverts.Average(a => a.CPM) : 0;
		public int CPM { get; set; }
	}
}
