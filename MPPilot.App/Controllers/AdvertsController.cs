using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Services;
using MPPilot.Domain.Utils;

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

                adverts = adverts.OrderByDescending(advert => advert.LastUpdateDate).ToList();

                return View(adverts);
            }
            catch
            {
                return View(Enumerable.Empty<Advert>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Advert oldAdvert, Advert newAdvert)
        {
            if (oldAdvert.AdvertId != newAdvert.AdvertId)
                throw new Exception($"Id старой РК ({oldAdvert.AdvertId}) и обновленной РК ({newAdvert.AdvertId}) не могут отличаться.");

            var changeSettingsTasks = new List<Task>();
            if (oldAdvert.Name != newAdvert.Name)
                changeSettingsTasks.Add(_wildberriesService.RenameAdvert(newAdvert.AdvertId, newAdvert.Name));

            if (oldAdvert.Keyword != newAdvert.Keyword)
                changeSettingsTasks.Add(_wildberriesService.ChangeAdvertKeyword(newAdvert.AdvertId, newAdvert.Keyword));

            if (oldAdvert.IsEnabled != newAdvert.IsEnabled)
                changeSettingsTasks.Add(_wildberriesService.ChangeAdvertStatus(newAdvert.AdvertId, newAdvert.Status));

            await Task.WhenAll(changeSettingsTasks);

            return Json(new { success = true });
		}
    }
}
