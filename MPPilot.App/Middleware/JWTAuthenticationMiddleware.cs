using MPPilot.Domain.Services.Token;

namespace MPPilot.App.Middleware
{
    public class JWTAuthenticationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ITokenService _tokenService;

		public JWTAuthenticationMiddleware(RequestDelegate next, ITokenService tokenService)
		{
			_next = next;
			_tokenService = tokenService;
		}

		public async Task Invoke(HttpContext context)
		{
			if (!context.Request.Path.StartsWithSegments("/Account", StringComparison.OrdinalIgnoreCase))
			{
				var token = context.Request.Cookies["Bearer"];
				var isValidToken = _tokenService.ValidateToken(token);

				if (!isValidToken)
				{
					context.Response.Redirect("/Account/Login");
					return;
				}

				context.Request.Headers.Add("Authorization", $"Bearer {token}");
			}
			
			await _next(context);
		}
	}
}
