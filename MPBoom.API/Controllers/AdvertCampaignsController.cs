using Microsoft.AspNetCore.Mvc;
using MPBoom.Core.Models;
using MPBoom.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace MPBoom.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdvertCampaignsController : ControllerBase
	{
		private readonly ILogger<AdvertCampaignsController> _logger;
		private readonly AdvertCampaignsLoaderService _advertPricesLoaderService;
		private readonly AdvertCampaignsBidService _advertBidService;
		private readonly WildberriesService _wildberriesService;

		public AdvertCampaignsController(ILogger<AdvertCampaignsController> logger,
			AdvertCampaignsLoaderService advertPricesLoaderService,
			AdvertCampaignsBidService advertCampaignsBidService,
			WildberriesService wildberriesService)
		{
			_logger = logger;
			_advertPricesLoaderService = advertPricesLoaderService;
			_advertBidService = advertCampaignsBidService;
			_wildberriesService = wildberriesService;

			_wildberriesService.SetApiKey("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6IjExYTEzYjJhLTBjY2ItNDhhYS04NjE1LTYyNDg3NmY4MzdjZSJ9.0Lhiz7X_SjLE-kOqXEJ7BEIVdH673sbpVMfuV9VyX5M");
		}

		[HttpGet]
		[Route("getAverageCpm")]
		public async Task<IActionResult> GetAverageCPM([Required] string keyword)
		{
			return Ok(await _advertBidService.GetAverageCPM(keyword));
		}

		[HttpPost]
		[Route("addSeller")]
		public async Task<IActionResult> AddSeller([Required] string apiKey)
		{
			if (_advertPricesLoaderService.AddSellerKey(apiKey))
				return Ok("Новый поставщик успешно добавлен.");
			else
				return Ok("Такой поставщик уже существует.");
		}

		[HttpGet]
		[Route("getAdvertCampaigns")]
		public async Task<IActionResult> GetAdvertCampaigns([Required] string apiKey)
		{
			return Ok(_advertPricesLoaderService.GetAdvertCampaigns(apiKey));
		}

		[HttpGet]
		[Route("getAdvertCampaignSearchInfo")]
		public async Task<IActionResult> GetAdvertCampaignSearchInfo([Required] string advertId, [Required] string keyword)
		{
			var result = await _advertBidService.GetAdvertCampaignsStatistics(advertId, keyword);
			if (result is not null)
				return Ok(result);
			else
				return NotFound("Ваша рекламная кампания не найдена на первых 5-и страницах.");
		}

		[HttpPost]
		[Route("changeCPM")]
		public async Task<IActionResult> ChangeCPM([Required] AdvertCampaign advertCampaign, [Required] int newCPM)
		{
			var result = await _wildberriesService.ChangeCPM(advertCampaign, newCPM);
			return Ok(result);
		}
	}
}
