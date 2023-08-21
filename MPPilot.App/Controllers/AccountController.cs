using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Models.Accounts;
using MPPilot.Domain.Services.Accounts;
using MPPilot.Domain.Services.Token;

namespace MPPilot.App.Controllers
{
	public class AccountController : Controller
	{
		private readonly AccountsService _accountService;

		public AccountController(AccountsService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(Account account)
		{
			try
			{
				await _accountService.RegisterAsync(account);
				return RedirectToAction("Login");
			}
			catch (UserAlreadyExistsException ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			return View(account);
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(Account account)
		{
			try
			{
				var token = await _accountService.LoginAsync(account.Email, account.Password);
                SaveTokenToCookie(token);

				return RedirectToAction("Index", "Home");
			}
			catch (ArgumentException ex)
			{
				ModelState.Clear();
				ModelState.AddModelError(string.Empty, ex.Message);
				return View(account);
			}
		}

        private void SaveTokenToCookie(string token)
        {
            Response.Cookies.Append(JwtBearerDefaults.AuthenticationScheme, token);
        }
	}
}
