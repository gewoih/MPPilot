using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Models.Autobidders;
using MPPilot.Domain.Services.Autobidders;

namespace App.Controllers
{
    public class AutobidderController : Controller
    {
        private readonly AutobidderService _autobidderService;

        public AutobidderController(AutobidderService autobidderService)
        {
            _autobidderService = autobidderService;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Autobidder autobidder)
        {
            await _autobidderService.Update(autobidder);
            return Ok();
        }
    }
}
