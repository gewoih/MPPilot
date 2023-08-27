﻿using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Services.Accounts;
using MPPilot.Domain.Services.Dashboards;

namespace MPPilot.App.Controllers
{
	public class DashboardController : Controller
	{
		private readonly DashboardService _dashboardService;
		private readonly IUsersService _accountsService;

		public DashboardController(DashboardService dashboardService, IUsersService accountsService)
		{
			_dashboardService = dashboardService;
			_accountsService = accountsService;
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}
