using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPPilot.App.Models;
using MPPilot.Domain.Services.Account;

namespace MPPilot.App.Controllers
{
    [Authorize]
	public class SettingsController : Controller
    {
        private readonly AccountsService _accountsService;

        public SettingsController(AccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(AccountSettingsDTO settings)
        {
			var isSaved = await _accountsService.SaveSettings(settings);
                
            if (isSaved)
                return RedirectToAction("Index", "Adverts");
            else
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка при сохранении API-ключа");
                return View("Index", settings);
            }
        }
    }
}
