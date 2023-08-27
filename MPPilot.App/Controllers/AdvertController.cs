﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPPilot.App.Models;
using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Services.Autobidders;
using MPPilot.Domain.Services.Marketplaces;

namespace MPPilot.App.Controllers
{
	[Authorize]
	public class AdvertController : Controller
	{
		private readonly WildberriesService _wildberriesService;
		private readonly IAutobiddersService _autobiddersService;

		public AdvertController(WildberriesService wildberriesService, IAutobiddersService autobidderService)
		{
			_wildberriesService = wildberriesService;
			_autobiddersService = autobidderService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<List<Advert>> GetAdverts()
		{
			var adverts = await _wildberriesService.GetActiveAdvertsAsync(withInfo: true, withKeywords: true, withStatistics: true);
			await _autobiddersService.LoadAutobiddersForAdverts(adverts);

			adverts = adverts
						.OrderByDescending(advert => advert.IsAutobidderEnabled)
						.ThenByDescending(advert => advert.CreatedDate)
						.ToList();

			return adverts;
		}

		[HttpPost]
		public async Task<IActionResult> Edit([FromBody] AdvertEditModel advertEditModel)
		{
			var advertId = advertEditModel.AdvertId;

			var changeSettingsTasks = new List<Task>();
			if (advertEditModel.NewName is not null)
				changeSettingsTasks.Add(_wildberriesService.RenameAdvert(advertId, advertEditModel.NewName));

			if (advertEditModel.NewKeyword is not null)
				changeSettingsTasks.Add(_wildberriesService.ChangeAdvertKeyword(advertId, advertEditModel.NewKeyword));

			if (advertEditModel.IsEnabled.HasValue)
			{
				var isEnabled = advertEditModel.IsEnabled.Value;
				if (isEnabled)
					changeSettingsTasks.Add(_wildberriesService.StartAdvertAsync(advertId));
				else
                    changeSettingsTasks.Add(_wildberriesService.StopAdvertAsync(advertId));
            }

			await Task.WhenAll(changeSettingsTasks);

			return Ok();
		}
	}
}
