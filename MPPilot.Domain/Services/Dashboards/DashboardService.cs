﻿using MPPilot.Domain.Models.Dashboards;
using MPPilot.Domain.Services.Marketplaces;

namespace MPPilot.Domain.Services.Dashboards
{
	public class DashboardService
	{
		private readonly WildberriesService _wildberriesService;

		public DashboardService(WildberriesService wildberriesService)
		{
			_wildberriesService = wildberriesService;
		}

		public async Task<DashboardStatistics> GetStatistics()
		{
			var adverts = await _wildberriesService.GetActiveAdvertsAsync(withStatistics: true);

			var statistics = new DashboardStatistics
			{
				TotalInAdverts = adverts.Sum(advert => advert.BudgetSize)
			};

			return statistics;
		}
	}
}
