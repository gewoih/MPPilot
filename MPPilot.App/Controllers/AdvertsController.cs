using Microsoft.AspNetCore.Mvc;
using MPPilot.App.Models;
using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Services;
using MPPilot.Domain.Services.Autobidders;

namespace MPPilot.App.Controllers
{
	public class AdvertsController : Controller
	{
		private readonly WildberriesService _wildberriesService;
		private readonly AccountsService _accountService;
		private readonly AutobidderService _autobidderService;

		public AdvertsController(WildberriesService wildberriesService, AccountsService accountsService, AutobidderService autobidderService)
		{
			_wildberriesService = wildberriesService;
			_accountService = accountsService;
			_autobidderService = autobidderService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<List<Advert>> GetAdverts()
		{
			\var accountSettings = await _accountService.GetCurrentAccountSettings();

			var adverts = new List<Advert>();
			var searchAdvertsTask = _wildberriesService.GetAdvertsAsync(accountSettings.WildberriesApiKey, type: AdvertType.Search);
			var productPageAdvertsTask = _wildberriesService.GetAdvertsAsync(accountSettings.WildberriesApiKey, type: AdvertType.ProductPage);
			await Task.WhenAll(searchAdvertsTask, productPageAdvertsTask)
				.ContinueWith(task =>
				{
					foreach (var list in task.Result)
					{
						adverts.AddRange(list);
					}
				});

			//Заполнение автобиддеров, возможна оптимизация
			foreach (var advert in adverts)
			{
				advert.Autobidder = await _autobidderService.GetByAdvert(advert.AdvertId);
			}

			adverts = adverts
						.OrderByDescending(advert => advert.IsAutobidderEnabled)
						.ThenByDescending(advert => advert.CreatedDate)
						.ToList();

			return adverts;
		}

		[HttpPost]
		public async Task<IActionResult> Edit([FromBody] AdvertEditModel advertEditModel)
		{
			var accountSettings = await _accountService.GetCurrentAccountSettings();
			var apiKey = accountSettings.WildberriesApiKey;

			var oldAdvert = advertEditModel.OldAdvert;
			var newAdvert = advertEditModel.NewAdvert;

			if (oldAdvert.AdvertId != newAdvert.AdvertId)
				throw new Exception($"Id старой РК ({oldAdvert.AdvertId}) и обновленной РК ({newAdvert.AdvertId}) не могут отличаться.");

			var changeSettingsTasks = new List<Task>();
			if (oldAdvert.Name != newAdvert.Name)
				changeSettingsTasks.Add(_wildberriesService.RenameAdvert(apiKey, newAdvert.AdvertId, newAdvert.Name));

			if (oldAdvert.Keyword != newAdvert.Keyword)
				changeSettingsTasks.Add(_wildberriesService.ChangeAdvertKeyword(apiKey, newAdvert.AdvertId, newAdvert.Keyword));

			if (oldAdvert.IsEnabled != newAdvert.IsEnabled)
				changeSettingsTasks.Add(_wildberriesService.ChangeAdvertStatus(apiKey, newAdvert.AdvertId, newAdvert.Status));

			await Task.WhenAll(changeSettingsTasks);

			return Json(new { success = true });
		}
	}
}
