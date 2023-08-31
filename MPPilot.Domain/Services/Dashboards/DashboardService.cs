using MPPilot.Domain.Models.Dashboards;
using MPPilot.Domain.Services.Autobidders;
using MPPilot.Domain.Services.Marketplaces;

namespace MPPilot.Domain.Services.Dashboards
{
	public class DashboardService
	{
		private readonly WildberriesService _wildberriesService;
		private readonly IAutobiddersService _autobiddersService;

		public DashboardService(WildberriesService wildberriesService, IAutobiddersService autobiddersService)
		{
			_wildberriesService = wildberriesService;
			_autobiddersService = autobiddersService;
		}

		public async Task<WildberriesAdvertsStatistics> GetStatistics()
		{
			var adverts = await _wildberriesService.GetActiveAdvertsAsync(withBudget: true, withStatistics: true);
			await _autobiddersService.LoadAutobiddersForAdvertsAsync(adverts);

			var balance = await _wildberriesService.GetBalance();

			var statistics = new WildberriesAdvertsStatistics
			{
				ActiveAdvertsCount = adverts.Count,
				ActiveAutobiddersCount = adverts.Count(advert => advert.IsAutobidderEnabled),
				WildberriesBalance = balance,
				TotalBudgetInAdverts = adverts.Sum(advert => advert.Balance),
				TotalBudgetInAutobidders = adverts.Where(advert => advert.IsAutobidderEnabled).Sum(advert => advert.Balance)
			};

			return statistics;
		}
	}
}
