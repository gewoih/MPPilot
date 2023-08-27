using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MPPilot.Domain.Models.Users;
using MPPilot.Domain.Services.Accounts;

namespace MPPilot.App.Middleware
{
	public class AuthenticationMiddleware : IMiddleware
	{
		private readonly IUsersService _accountsService;
		private readonly UserManager<User> _userManager;

		public AuthenticationMiddleware(IUsersService accountsService, UserManager<User> userManager)
		{
			_accountsService = accountsService;
			_userManager = userManager;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var currentUser = context.User;
			if (currentUser.Identity.IsAuthenticated)
			{
				if (await _accountsService.GetCurrentUserAsync() is null)
				{
					var user = await _userManager.Users
									.Include(user => user.Settings)
									.SingleAsync(user => user.UserName == currentUser.Identity.Name);

					await _accountsService.SetCurrentUserAsync(user);
				}
			}
			
			await next(context);
		}
	}
}
