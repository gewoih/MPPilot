using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MPBoom.Core.Models;
using System.Diagnostics;

namespace MPBoom.Core.Services
{
	public class AdvertCampaignsLoaderService
	{
		private readonly Dictionary<string, List<AdvertCampaign>> _advertCampaigns;
		private readonly WildberriesService _wbService;
		private readonly ILogger<AdvertCampaignsLoaderService> _logger;
		private readonly TimeSpan _updateInterval;

		public AdvertCampaignsLoaderService(ILogger<AdvertCampaignsLoaderService> logger, IConfiguration configuration, WildberriesService wbService)
		{
			_wbService = wbService;
			_logger = logger;
			_updateInterval = TimeSpan.FromSeconds(int.Parse(configuration["AdvertCampaignsUpdateInterval"]));
			_advertCampaigns = new();

			_ = Task.Run(StartUpdateCampaigns);
		}

		public bool AddSellerKey(string apiKey)
		{
			if (!_advertCampaigns.ContainsKey(apiKey))
			{
				_advertCampaigns[apiKey] = new List<AdvertCampaign>();
				_logger.LogInformation("Был добавлен новый API-ключ для автоматического обновления рекламных кампаний.");

				return true;
			}

			return false;
		}

		public bool IsSellerExists(string apiKey)
		{
			return _advertCampaigns.ContainsKey(apiKey);
		}

		public List<AdvertCampaign> GetAdvertCampaigns(string apiKey)
		{
			if (_advertCampaigns.ContainsKey(apiKey))
				return _advertCampaigns[apiKey];
			else
				return new List<AdvertCampaign>();
		}

		private async Task StartUpdateCampaigns()
		{
			_logger.LogInformation("Старт обновления рекламных кампаний.");

			while (true)
			{
				var sellersCount = _advertCampaigns.Count;

				if (sellersCount > 0)
				{
					_logger.LogInformation($"Запущено обновление рекламных кампаний для {_advertCampaigns.Count} клиентов.");
					var generalStopWatch = Stopwatch.StartNew();

					foreach (var apiKey in _advertCampaigns.Keys.ToList())
					{
						_wbService.SetApiKey(apiKey);

						var advertCampaigns = await _wbService.GetAdvertCampaignsAsync();
						_advertCampaigns[apiKey] = new List<AdvertCampaign>(advertCampaigns);
					}

					_logger.LogInformation($"Обновление рекламных кампаний завершено за {generalStopWatch.Elapsed}");
				}

				await Task.Delay(_updateInterval);
			}
		}
	}
}
