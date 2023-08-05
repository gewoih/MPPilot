using Microsoft.AspNetCore.Mvc;
using MPBoom.Domain.Models.Advert;
using MPBoom.Domain.Models;
using MPBoom.Domain.Services.API;

namespace MPPilot.App.Controllers
{
    public class AdvertsController : Controller
    {
        private readonly WildberriesService _wildberriesService;
        private readonly IConfiguration _configuration;
        private UserSettings? _userSettings;
        private IEnumerable<Advert> _adverts;

        public AdvertsController(WildberriesService wildberriesService, 
            IConfiguration configuration) 
        {
            _wildberriesService = wildberriesService;
            _configuration = configuration;

            _wildberriesService.SetApiKey("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6IjExYTEzYjJhLTBjY2ItNDhhYS04NjE1LTYyNDg3NmY4MzdjZSJ9.0Lhiz7X_SjLE-kOqXEJ7BEIVdH673sbpVMfuV9VyX5M");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var isValidApiKey = false;

            if (isValidApiKey)
            {
                var searchAdverts = await _wildberriesService.GetAdvertsAsync(type: AdvertType.Search);
                var productPageAdverts = await _wildberriesService.GetAdvertsAsync(type: AdvertType.ProductPage);

                _adverts = searchAdverts.Concat(productPageAdverts);

                return View(_adverts);
            }
            else
                return View("APIKeyError");
        }
    }
}
