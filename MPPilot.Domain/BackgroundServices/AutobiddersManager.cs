using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Services.Autobidders;
using MPPilot.Domain.Services.Marketplaces;
using System.Diagnostics;

namespace MPPilot.Domain.BackgroundServices
{
    public class AutobiddersManager : BackgroundService
    {
        private AutobidderService _autobidderService;
        private WildberriesService _wildberriesService;
        private readonly AdvertsMarketService _advertsMarketService;
        private readonly ILogger<AutobiddersManager> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly Stopwatch _stopwatch;

        public AutobiddersManager(AdvertsMarketService advertsMarketService,
            ILogger<AutobiddersManager> logger,
            IServiceProvider serviceProvider)
        {
            _advertsMarketService = advertsMarketService;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
            _logger.LogInformation($"Запущена фоновая служба {nameof(AutobiddersManager)}");

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                _stopwatch.Restart();

                using var scope = _serviceProvider.CreateScope();

                _wildberriesService = scope.ServiceProvider.GetRequiredService<WildberriesService>();
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
            }

            _logger.LogInformation($"Остановлена фоновая служба {nameof(AutobiddersManager)}");
		}

        private async Task HandleConservativeAutobidder(Autobidder autobidder)
        {
            var apiKey = autobidder.Account.Settings.WildberriesApiKey;
            _wildberriesService.SetApiKey(apiKey);

            var advertTask = _wildberriesService.GetAdvertWithKeywordAndCPM(autobidder.AdvertId);
            var advertTodayExpensesTask = _wildberriesService.GetExpensesForToday(autobidder.AdvertId);
            await Task.WhenAll(advertTask, advertTodayExpensesTask);

            var advert = advertTask.Result;
            var advertTodayExpenses = advertTodayExpensesTask.Result;

            //Если превышение по бюджету - ставим ставки на паузу и останавливаем РК
            if (advertTodayExpenses >= autobidder.DailyBudget)
            {
                await _autobidderService.PauseBids(autobidder, DateTime.UtcNow.Date.AddDays(1));
                _logger.LogInformation("Превышение по бюджету для РК '{AdvertId}'. Ставки по автобиддеру приостановлены до следующего дня.", advert.AdvertId);

                await _wildberriesService.StopAdvertAsync(advert.AdvertId);
                _logger.LogInformation("Рекламная кампания {AdvertId} приостановлена автобиддером.", advert.AdvertId);

                return;
            }
            else if (autobidder.BidsPausedTill is not null)
            {
                await _autobidderService.StartBids(autobidder);
                _logger.LogInformation("Ставки по автобиддеру возобновлены для РК {AdvertId}.", advert.AdvertId);

                await _wildberriesService.StartAdvertAsync(advert.AdvertId);
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
                var isCpmChanged = await _wildberriesService.ChangeCPM(advert, targetAdvert.CPM);
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