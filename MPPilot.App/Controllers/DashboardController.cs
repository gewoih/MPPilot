using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Services.Accounts;
using MPPilot.Domain.Services.Dashboards;

namespace MPPilot.App.Controllers
{
	public class DashboardController : Controller
	{
		private readonly DashboardService _dashboardService;
		private readonly AccountsService _accountsService;

		public DashboardController(DashboardService dashboardService, AccountsService accountsService)
		{
			_dashboardService = dashboardService;
			_accountsService = accountsService;
		}

		public async Task<IActionResult> Index()
		{
			var currentAccountSettings = _accountsService.GetCurrentAccountSettings();
			var dashboardStatistics = await _dashboardService.GetStatistics(currentAccountSettings.WildberriesApiKey);

			return View();
		}
	}
}
