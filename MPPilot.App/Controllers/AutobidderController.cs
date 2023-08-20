using Microsoft.AspNetCore.Mvc;
using MPPilot.App.Models;
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
		public async Task<IActionResult> Edit([FromBody] AutobidderEditModel autobidderEditModel)
		{
			if (autobidderEditModel is null)
				return BadRequest();

			var autobidder = new Autobidder
			{
				Id = (Guid)autobidderEditModel.Id,
				AdvertId = (int)autobidderEditModel.AdvertId,
				IsEnabled = (bool)autobidderEditModel.IsEnabled,
				DailyBudget = (double)autobidderEditModel.DailyBudget
			};

			if (autobidder.Id != Guid.Empty)
				await _autobidderService.Update(autobidder);
			else
				await _autobidderService.Create(autobidder);

			return Ok();
		}

		[HttpGet]
		public async Task<List<AdvertBid>> GetBids(Guid autobidderId)
		{
			return await _autobidderService.GetBids(autobidderId);
		}
	}
}
