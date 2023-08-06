using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Models.Accounts;
using MPPilot.Domain.Services;
using MPPilot.Domain.Services.Token;

namespace MPPilot.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountsService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(AccountsService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
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
                var isRegistered = await _accountService.RegisterAsync(account);
                if (!isRegistered)
                    ModelState.AddModelError(string.Empty, "Неизвестная ошибка при регистрации пользователя.");
					
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
            var identity = await _accountService.LoginAsync(account.Email, account.Password);

            if (identity is null)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль!");
                return View(account);
            }
            else
            {
                var token = _tokenService.GenerateToken(identity);
                SaveTokenToCookie(token);

                return RedirectToAction("Index", "Settings");
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
