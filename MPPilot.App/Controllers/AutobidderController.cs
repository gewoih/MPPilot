using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Models.Autobidders;
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

		[HttpPost]
		public async Task<IActionResult> Edit([FromBody] Autobidder autobidder)
		{
			await _autobidderService.Update(autobidder);
			return Ok();
		}
	}
}
