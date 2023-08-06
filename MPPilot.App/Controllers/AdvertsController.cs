using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Services;

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
                var adverts = new List<Advert>();
                var searchAdvertsTask = _wildberriesService.GetAdvertsAsync(type: AdvertType.Search);
                var productPageAdvertsTask = _wildberriesService.GetAdvertsAsync(type: AdvertType.ProductPage);
                await Task.WhenAll(searchAdvertsTask, productPageAdvertsTask)
                    .ContinueWith(task =>
                    {
                        foreach (var list in task.Result)
                        {
                            adverts.AddRange(list);
                        }
                    });

                return View(adverts);
            }
            catch
            {
                return View(Enumerable.Empty<Advert>());
            }
        }
    }
}
