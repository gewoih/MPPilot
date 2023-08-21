using Microsoft.AspNetCore.Authentication;
using MPPilot.Domain.Services.Accounts;
using MPPilot.Domain.Services.Token;
using System.Security.Claims;

namespace MPPilot.App.Middleware
{
    public class AuthenticationMiddleware : IMiddleware
	{
		private readonly ITokenService _tokenService;
		private readonly AccountsService _accountsService;

		public AuthenticationMiddleware(ITokenService tokenService, AccountsService accountsService)
		{
			_tokenService = tokenService;
			_accountsService = accountsService;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (!context.Request.Path.StartsWithSegments("/Account", StringComparison.OrdinalIgnoreCase))
			{
				var token = await context.GetTokenAsync("Bearer", "access_token");
				var isValidToken = _tokenService.ValidateToken(token);

				if (!isValidToken)
				{
					context.Response.Redirect("/Account/Login");
					return;
				}
				else if (_accountsService.GetCurrentAccount() is null)
				{
					var currentUser = context.User;
					var claimId = currentUser.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));
					if (claimId is not null)
					{
						var accountId = Guid.Parse(claimId.Value);
						await _accountsService.SetCurrentAccount(accountId);
					}
				}
			}
			
			await next(context);
		}
	}
}
