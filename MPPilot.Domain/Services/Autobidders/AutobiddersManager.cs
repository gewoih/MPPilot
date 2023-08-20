using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Services.Marketplaces;
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
						_logger.LogInformation("Найдено {Count} активных автобиддеров. Начинаем обработку...", autobidders.Count);
						foreach (var autobidder in autobidders)
						{
							if (autobidder.Mode == AutobidderMode.Conservative)
								await HandleConservativeAutobidder(autobidder);

							_logger.LogInformation("Завершена обработка автобиддера (тип = {Mode}, id = {Id}) для РК '{AdvertId}'", autobidder.Mode, autobidder.Id, autobidder.AdvertId);
						}
					}
					else
						_logger.LogInformation($"Активные автобиддеры не найдены...");

					_logger.LogInformation("Обработка всех автобиддеров заняла: {Elapsed}", _stopwatch.Elapsed);

					await Task.Delay(TimeSpan.FromSeconds(10));
				}
			});
		}

		private async Task HandleConservativeAutobidder(Autobidder autobidder)
		{
			var apiKey = autobidder.Account.Settings.WildberriesApiKey;

			var advertTask = _wildberriesService.GetAdvertWithKeywordAndCPM(apiKey, autobidder.AdvertId);
			var advertTodayExpensesTask = _wildberriesService.GetExpensesForToday(apiKey, autobidder.AdvertId);
			await Task.WhenAll(advertTask, advertTodayExpensesTask);

			var advert = advertTask.Result;
			var advertTodayExpenses = advertTodayExpensesTask.Result;

			//Если превышение по бюджету - ставим ставки на паузу и останавливаем РК
			if (advertTodayExpenses >= autobidder.DailyBudget)
			{
				await _autobidderService.PauseBids(autobidder, DateTime.UtcNow.Date.AddDays(1));
				_logger.LogInformation("Превышение по бюджету для РК '{AdvertId}'. Ставки по автобиддеру приостановлены до следующего дня.", advert.AdvertId);

				await _wildberriesService.StopAdvertAsync(apiKey, advert.AdvertId);
				_logger.LogInformation("Рекламная кампания {AdvertId} приостановлена автобиддером.", advert.AdvertId);

				return;
			}
			else if (autobidder.BidsPausedTill is not null)
			{
				await _autobidderService.StartBids(autobidder);
				_logger.LogInformation("Ставки по автобиддеру возобновлены для РК {AdvertId}.", advert.AdvertId);

				await _wildberriesService.StartAdvertAsync(apiKey, advert.AdvertId);
				_logger.LogInformation("Рекламная кампания {AdvertId} возобновлена автобиддером.", advert.AdvertId);
			}

			var advertMarketStatistics = await _advertsMarketService.GetAdvertMarketStatistics(advert.Keyword, advert.AdvertId);
			var averageCpm = (int)advertMarketStatistics.AverageCPM;

			var targetAdvert = advertMarketStatistics.MarketAdverts.FirstOrDefault(a => a.CPM < averageCpm);
			if (targetAdvert is null)
				return;

			var currentPosition = advertMarketStatistics.AdvertPosition;

			//Если CPM понижается, то мы делаем это без дополнительных проверок
			//Если CPM повышается, то мы должны убедиться, что мы УЖЕ не находимся в целевых границах
			if (advert.CPM < targetAdvert.CPM && currentPosition != targetAdvert.Position || advert.CPM > targetAdvert.CPM)
			{
				var isCpmChanged = await _wildberriesService.ChangeCPM(apiKey, advert, targetAdvert.CPM);
				if (isCpmChanged)
				{
					var newBid = new AdvertBid
					{
						AdvertKeyword = advert.Keyword,
						AdvertPosition = currentPosition,
						AutobidderMode = AutobidderMode.Conservative,
						LastCPM = advert.CPM,
						CurrentCPM = targetAdvert.CPM,
						TargetPositionLeftBound = targetAdvert.Position,
						TargetPositionRightBound = targetAdvert.Position,
						Reason = ChangeBidReason.NotAverageCpm
					};

					await _autobidderService.AddBid(autobidder, newBid);
				}
			}
		}
	}
}