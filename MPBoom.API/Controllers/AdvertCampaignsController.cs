using Microsoft.AspNetCore.Mvc;
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

		public AdvertCampaignsController(ILogger<AdvertCampaignsController> logger,
			AdvertCampaignsLoaderService advertPricesLoaderService,
			AdvertCampaignsBidService advertCampaignsBidService)
		{
			_logger = logger;
			_advertPricesLoaderService = advertPricesLoaderService;
			_advertBidService = advertCampaignsBidService;
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
	}
}
