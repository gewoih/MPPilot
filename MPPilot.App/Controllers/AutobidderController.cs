using Microsoft.AspNetCore.Mvc;
using MPPilot.App.Models;
using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Services.Autobidders;

namespace MPPilot.App.Controllers
{
	public class AutobidderController : Controller
	{
		private readonly IAutobiddersService _autobiddersService;

		public AutobidderController(IAutobiddersService autobidderService) 
		{
			_autobiddersService = autobidderService;
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
				await _autobiddersService.UpdateAsync(autobidder);
			else
				await _autobiddersService.CreateAsync(autobidder);

			return Ok();
		}

		[HttpGet]
		public async Task<List<AdvertBid>> GetBids(Guid autobidderId)
		{
			return await _autobiddersService.GetBidsAsync(autobidderId);
		}
	}
}
