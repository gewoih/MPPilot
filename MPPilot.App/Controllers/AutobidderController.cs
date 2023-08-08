using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Services.Autobidders;

namespace MPPilot.App.Controllers
{
	public class AutobidderController : Controller
	{
		private readonly AutobidderService _autobidderService;

		public AutobidderController(AutobidderService autobidderService) 
		{
			_autobidderService = autobidderService;
		}

		[HttpGet]
		public async Task<IActionResult> GetByAdvertId(int advertId)
		{
			try
			{
				var autobidder = await _autobidderService.GetByAdvert(advertId);
				return View();
			}
			catch (ArgumentException ex)
			{
				return RedirectToAction("Create");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create(int advertId)
		{
			var autobidder = await _autobidderService.Create(advertId);
			return View();
		}
	}
}
