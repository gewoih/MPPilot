using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Models.Autobidders;

namespace MPPilot.Domain.Services.Autobidders
{
	public class AutobiddersManager
	{
		private readonly WildberriesService _wildberriesService;
		private readonly AdvertsMarketService _advertsMarketService;
		private readonly ILogger<AutobiddersManager> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IServiceProvider _serviceProvider;

		public AutobiddersManager(WildberriesService wildberriesService,
			AdvertsMarketService advertsMarketService,
			ILogger<AutobiddersManager> logger,
			IHttpContextAccessor httpContextAccessor,
			IServiceProvider serviceProvider)
		{
			_wildberriesService = wildberriesService;
			_advertsMarketService = advertsMarketService;
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
			_serviceProvider = serviceProvider;
		}

		public void StartManagement()
		{
			_ = Task.Run(async () =>
			{
				while (true)
				{
					using var scope = _serviceProvider.CreateScope();

					var autobiddersService = scope.ServiceProvider.GetRequiredService<AutobidderService>();
					var autobidders = await autobiddersService.GetActiveAutobidders();

					if (autobidders.Any())
					{
						_logger.LogInformation($"Найдено {autobidders.Count} активных автобиддеров. Начинаем обработку...");
						foreach (var autobidder in autobidders)
						{
							if (autobidder.Mode == AutobidderMode.Conservative)
								await HandleConservativeAutobidder(autobidder, autobiddersService);

							_logger.LogInformation($"Завершена обработка автобиддера (тип = {autobidder.Mode}, id = {autobidder.Id}) для РК '{autobidder.AdvertId}'");
						}
					}
					else
						_logger.LogInformation($"Активные автобиддеры не найдены...");

					await Task.Delay(TimeSpan.FromSeconds(10));
				}
			});
		}

		private async Task HandleConservativeAutobidder(Autobidder autobidder, AutobidderService autobidderService)
		{
			_wildberriesService.SetApiKey("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6IjExYTEzYjJhLTBjY2ItNDhhYS04NjE1LTYyNDg3NmY4MzdjZSJ9.0Lhiz7X_SjLE-kOqXEJ7BEIVdH673sbpVMfuV9VyX5M");
			var adverts = await _wildberriesService.GetAdvertsAsync();
			var advert = adverts.First(a => a.AdvertId == autobidder.AdvertId);

			var advertMarketStatistics = await _advertsMarketService.GetAdvertMarketStatistics(advert.Keyword, advert.AdvertId);
			var averageCpm = (int)advertMarketStatistics.AverageCPM;

			var targetAdvert = advertMarketStatistics.MarketAdverts.First(a => a.CPM < averageCpm);

			if (advert.CPM != targetAdvert.CPM)
			{
				var isCpmChanged = await _wildberriesService.ChangeCPM(advert, targetAdvert.CPM);
				if (isCpmChanged)
				{
					var newBid = new AdvertBid
					{
						AdvertKeyword = advert.Keyword,
						AdvertPosition = advertMarketStatistics.AdvertPosition,
						AutobidderMode = AutobidderMode.Conservative,
						LastCPM = advert.CPM,
						CurrentCPM = targetAdvert.CPM,
						TargetPositionLeftBound = targetAdvert.Position,
						TargetPositionRightBound = targetAdvert.Position,
						Reason = ChangeBidReason.BelowAverageCpm
					};

					await autobidderService.AddBid(autobidder, newBid);
				}
			}
		}
	}
}