using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Models.Autobidders;
using System.Diagnostics;

namespace MPPilot.Domain.Services.Autobidders
{
	public class AutobiddersManager
	{
		private AutobidderService _autobidderService;
		private readonly WildberriesService _wildberriesService;
		private readonly AdvertsMarketService _advertsMarketService;
		private readonly ILogger<AutobiddersManager> _logger;
		private readonly IServiceProvider _serviceProvider;
		private readonly Stopwatch _stopwatch;

		public AutobiddersManager(AdvertsMarketService advertsMarketService,
			ILogger<AutobiddersManager> logger,
			WildberriesService wildberriesService,
			IServiceProvider serviceProvider)
		{
			_advertsMarketService = advertsMarketService;
			_wildberriesService = wildberriesService;
			_serviceProvider = serviceProvider;
			_logger = logger;
			_stopwatch = new Stopwatch();
		}

		public void StartManagement()
		{
			_ = Task.Run(async () =>
			{
				while (true)
				{
					_stopwatch.Restart();

					using var scope = _serviceProvider.CreateScope();

					_autobidderService = scope.ServiceProvider.GetRequiredService<AutobidderService>();
					var autobidders = await _autobidderService.GetActiveAutobidders();

					if (autobidders.Any())
					{
						_logger.LogInformation($"Найдено {autobidders.Count} активных автобиддеров. Начинаем обработку...");
						foreach (var autobidder in autobidders)
						{
							if (autobidder.Mode == AutobidderMode.Conservative)
								await HandleConservativeAutobidder(autobidder);

							_logger.LogInformation($"Завершена обработка автобиддера (тип = {autobidder.Mode}, id = {autobidder.Id}) для РК '{autobidder.AdvertId}'");
						}
					}
					else
						_logger.LogInformation($"Активные автобиддеры не найдены...");

					_logger.LogInformation($"Обработка всех автобиддеров заняла: {_stopwatch.Elapsed}");

					await Task.Delay(TimeSpan.FromSeconds(10));
				}
			});
		}

		private async Task HandleConservativeAutobidder(Autobidder autobidder)
		{
			var apiKey = autobidder.Account.Settings.WildberriesApiKey;
			var advert = await _wildberriesService.GetAdvertWithKeywordAndCPM(apiKey, autobidder.AdvertId);

			var advertMarketStatistics = await _advertsMarketService.GetAdvertMarketStatistics(advert.Keyword, advert.AdvertId);
			var averageCpm = (int)advertMarketStatistics.AverageCPM;

			var targetAdvert = advertMarketStatistics.MarketAdverts.First(a => a.CPM < averageCpm);

			if (advert.CPM != targetAdvert.CPM)
			{
				var isCpmChanged = await _wildberriesService.ChangeCPM(apiKey, advert, targetAdvert.CPM);
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

					await _autobidderService.AddBid(autobidder, newBid);
				}
			}
		}
	}
}