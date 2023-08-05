using Microsoft.AspNetCore.Mvc;
using MPBoom.Domain.Models.Advert;
using MPBoom.Domain.Services.API;

namespace MPPilot.App.Controllers
{
    public class AdvertsController : Controller
    {
        private readonly WildberriesService _wildberriesService;

        public AdvertsController(WildberriesService wildberriesService) 
        {
            _wildberriesService = wildberriesService;
            _wildberriesService.SetApiKey("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6IjExYTEzYjJhLTBjY2ItNDhhYS04NjE1LTYyNDg3NmY4MzdjZSJ9.0Lhiz7X_SjLE-kOqXEJ7BEIVdH673sbpVMfuV9VyX5M");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var searchAdverts = await _wildberriesService.GetAdvertsAsync(type: AdvertType.Search);
                var productPageAdverts = await _wildberriesService.GetAdvertsAsync(type: AdvertType.ProductPage);

                var _adverts = searchAdverts.Concat(productPageAdverts);
                return View(_adverts);
            }
            catch
            {
                return View(Enumerable.Empty<Advert>());
            }
        }
    }
}
