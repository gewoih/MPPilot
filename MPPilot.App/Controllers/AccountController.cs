﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using MPBoom.Domain.Exceptions;
using MPBoom.Domain.Services.Security.Token;
using MPPilot.App.Models;
using MPPilot.App.Services;
using System.Security.Claims;

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
            var account = new AccountDTO();
            return View("Register", account);
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountDTO accountDTO)
        {
            try
            {
                var isRegistered = await _accountService.RegisterAsync(accountDTO);
                if (!isRegistered)
                    ModelState.AddModelError(string.Empty, "Неизвестная ошибка при регистрации пользователя.");
					
                return RedirectToAction("Login");
            }
            catch (UserAlreadyExistsException ex)
            {
				ModelState.AddModelError(string.Empty, ex.Message);
            }

			return View("Register", accountDTO);
		}

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountDTO accountDTO)
        {
            var identity = await _accountService.LoginAsync(accountDTO);

            if (identity is null)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль!");
                return View("Login", accountDTO);
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
