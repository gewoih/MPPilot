using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MPBoom.API.Services;
using MPBoom.Domain.Exceptions;
using MPBoom.Domain.Models.Token;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;

namespace MPBoom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountsService _accountsService;

        public AccountsController(AccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([Required] string name, [Required] string email, [Required] string password)
        {
            try
            {
                var isCreated = await _accountsService.RegisterAsync(name, email, password);

                if (isCreated)
                    return Ok(isCreated);
                else
                    return BadRequest(isCreated);
            }
            catch (UserAlreadyExistsException)
            {
                return BadRequest("Пользователь с таким Email уже найден в системе!");
            }
        }

        [HttpGet("/login")]
        public async Task<IActionResult> Login([Required] string email, [Required] string password)
        {
            var result = await _accountsService.LoginAsync(email, password);
            if (result)
                return Ok(result);
            else
                return BadRequest("Неверный Email или пароль!");
        }

        [HttpPost("/token")]
        public async Task<IActionResult> GetToken([Required] string email, [Required] string password)
        {
            var identity = await _accountsService.GetIdentityAsync(email, password);
            if (identity == null)
                return BadRequest("Неверный Email или пароль!");

            var dateNow = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: dateNow,
                claims: identity.Claims,
                expires: dateNow.Add(TimeSpan.FromMinutes(AuthOptions.LifetimeMinutes)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                Token = encodedJwt,
                Username = identity.Name
            };

            return Ok(response);
        }
    }
}
