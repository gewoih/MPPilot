using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Models.Accounts;
using MPPilot.Domain.Services;

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
        public async Task<IActionResult> Save(AccountSettings settings)
        {
            try
            {
                await _accountsService.SaveSettings(settings);
                return RedirectToAction("Index", "Adverts");
            }
            catch (APIKeyAlreadyExistsException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", settings);

            }
        }
    }
}
