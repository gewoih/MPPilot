using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Models.Accounts;
using MPPilot.Domain.Services.Accounts;

namespace MPPilot.App.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountsService _accountService;

		public AccountController(IAccountsService accountService)
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
				var credentials = new UserCredentials { Name = account.Name, Email = account.Email, Password = account.Password };

				await _accountService.RegisterAsync(credentials);
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
				var credentials = new UserCredentials { Email = account.Email, Password = account.Password };
				var token = await _accountService.LoginAsync(credentials);
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
			var cookieOptions = new CookieOptions
			{
				Expires = DateTime.UtcNow.AddHours(2),
				HttpOnly = true
			};

			Response.Cookies.Append(JwtBearerDefaults.AuthenticationScheme, token, cookieOptions);
		}
	}
}
