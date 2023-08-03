using Microsoft.AspNetCore.Mvc;
using MPBoom.Domain.Enums;
using MPBoom.Domain.Models;
using MPBoom.Domain.Services;
using System.ComponentModel.DataAnnotations;

namespace MPBoom.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdvertsController : ControllerBase
	{
		private readonly AdvertsBidService _advertBidService;
		private readonly WildberriesService _wildberriesService;
		private const string _apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6IjExYTEzYjJhLTBjY2ItNDhhYS04NjE1LTYyNDg3NmY4MzdjZSJ9.0Lhiz7X_SjLE-kOqXEJ7BEIVdH673sbpVMfuV9VyX5M";

		public AdvertsController(
			AdvertsBidService advertCampaignsBidService,
			WildberriesService wildberriesService)
		{
			_advertBidService = advertCampaignsBidService;
			_wildberriesService = wildberriesService;

			_wildberriesService.SetApiKey(_apiKey);
		}

		[HttpGet]
		[Route("getAverageCpm")]
		public async Task<IActionResult> GetAverageCPM([Required] string keyword)
		{
			return Ok(await _advertBidService.GetAverageCPM(keyword));
		}

		[HttpPost]
		[Route("changeCPM")]
		public async Task<IActionResult> ChangeCPM([Required] Advert advertCampaign, [Required] int newCPM)
		{
			var result = await _wildberriesService.ChangeCPM(advertCampaign, newCPM);
			return Ok(result);
		}

		[HttpPost]
		[Route("changeStatus")]
		public async Task<IActionResult> ChangeStatus([Required] int advertId, AdvertStatus status)
		{
			var result = await _wildberriesService.ChangeAdvertStatus(advertId, status);
			return Ok(result);
		}

		[HttpPost]
		[Route("renameAdvert")]
		public async Task<IActionResult> RenameAdvert([Required] int advertId, [Required] string name)
		{
			var result = await _wildberriesService.RenameAdvert(advertId, name);
			return Ok(result);
		}
	}
}
