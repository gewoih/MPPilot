using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPPilot.App.Services;

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
    }
}
