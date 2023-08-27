using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Models.Users;
using MPPilot.Domain.Services.Accounts;

namespace MPPilot.App.Controllers
{
	[Authorize]
	public class SettingsController : Controller
    {
        private readonly IUsersService _accountsService;

        public SettingsController(IUsersService accountsService)
        {
            _accountsService = accountsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserSettings settings)
        {
            try
            {
                await _accountsService.ChangeSettingsAsync(settings);
                return RedirectToAction("Index", "Advert");
            }
            catch (APIKeyAlreadyExistsException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", settings);

            }
        }
    }
}
