using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Services.Dashboards;

namespace MPPilot.App.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{
		private readonly DashboardService _dashboardService;

		public DashboardController(DashboardService dashboardService)
		{
			_dashboardService = dashboardService;
		}

		public async Task<IActionResult> Index()
		{
			var statistics = await _dashboardService.GetStatistics();
			return View(statistics);
		}
	}
}
