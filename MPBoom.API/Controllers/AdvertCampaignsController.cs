using Microsoft.AspNetCore.Mvc;
using MPBoom.Services.PricesLoader.Models;
using MPBoom.Services.PricesLoader.Services;
using System.ComponentModel.DataAnnotations;

namespace MPBoom.Services.PricesLoader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertCampaignsController : ControllerBase
    {
        private readonly ILogger<AdvertCampaignsController> _logger;
        private readonly AdvertCampaignsLoaderService _advertPricesLoaderService;

        public AdvertCampaignsController(ILogger<AdvertCampaignsController> logger, AdvertCampaignsLoaderService advertPricesLoaderService)
        {
            _logger = logger;
            _advertPricesLoaderService = advertPricesLoaderService;
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
    }
}
