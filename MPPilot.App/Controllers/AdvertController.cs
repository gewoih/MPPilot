﻿using Microsoft.AspNetCore.Mvc;
using MPPilot.App.Models;
using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Services.Accounts;
using MPPilot.Domain.Services.Autobidders;
using MPPilot.Domain.Services.Marketplaces;

namespace MPPilot.App.Controllers
{
	public class AdvertController : Controller
	{
		private readonly WildberriesService _wildberriesService;
		private readonly AccountsService _accountService;
		private readonly AutobidderService _autobidderService;

		public AdvertController(WildberriesService wildberriesService, AccountsService accountsService, AutobidderService autobidderService)
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
			var accountSettings = await _accountService.GetCurrentAccountSettings();

			var adverts = await _wildberriesService.GetActiveAdvertsAsync(accountSettings.WildberriesApiKey, 
								withInfo: true, 
								withKeywords: true, 
								withStatistics: true);

			adverts = await _autobidderService.LoadAutobidders(adverts);

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
			var advertId = advertEditModel.AdvertId;

			var changeSettingsTasks = new List<Task>();
			if (advertEditModel.NewName is not null)
				changeSettingsTasks.Add(_wildberriesService.RenameAdvert(apiKey, advertId, advertEditModel.NewName));

			if (advertEditModel.NewKeyword is not null)
				changeSettingsTasks.Add(_wildberriesService.ChangeAdvertKeyword(apiKey, advertId, advertEditModel.NewKeyword));

			if (advertEditModel.IsEnabled.HasValue)
			{
				var isEnabled = advertEditModel.IsEnabled.Value;
				if (isEnabled)
					changeSettingsTasks.Add(_wildberriesService.StartAdvertAsync(apiKey, advertId));
				else
                    changeSettingsTasks.Add(_wildberriesService.StopAdvertAsync(apiKey, advertId));
            }

			await Task.WhenAll(changeSettingsTasks);

			return Ok();
		}
	}
}