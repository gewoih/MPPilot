using Microsoft.AspNetCore.Mvc;
using MPBoom.API.Services;
using MPBoom.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

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

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([Required] string name, [Required] string email, [Required] string password)
        {
            try
            {
                var isCreated = await _accountsService.Register(name, email, password);

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

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login([Required] string email, [Required] string password)
        {
            var result = await _accountsService.Login(email, password);
            if (result)
                return Ok(result);
            else
                return BadRequest("Неверный Email или пароль!");
        }
    }
}
