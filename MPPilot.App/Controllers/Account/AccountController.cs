using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPPilot.App.Controllers.Account.Requests;
using MPPilot.Domain.Models.Users;

namespace MPPilot.App.Controllers.Account
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManger;

		public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_userManger = userManager;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterRequest request)
		{
			var user = new User
			{
				UserName = request.Email,
				Email = request.Email
			};

			var result = await _userManger.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				ModelState.Clear();
				if (result.Errors.Any())
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				else
					ModelState.AddModelError(string.Empty, "Данный аккаунт уже существует!");

				return View(request);
			}
			else
				return RedirectToAction("Login");
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest request)
		{
			var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, true, false);

			if (!result.Succeeded)
			{
				ModelState.Clear();
				ModelState.AddModelError(string.Empty, "Неправильный логин или пароль!");
				return View(request);
			}
			else
				return RedirectToAction("Index", "Home");
		}
	}
}
