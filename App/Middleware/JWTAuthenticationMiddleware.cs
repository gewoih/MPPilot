using MPPilot.Domain.Services.Token;

namespace App.Middleware
{
    public class JWTAuthenticationMiddleware : IMiddleware
    {
        private readonly ITokenService _tokenService;

        public JWTAuthenticationMiddleware(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Path.StartsWithSegments("/Account", StringComparison.OrdinalIgnoreCase))
            {
                var token = context.Request.Cookies["Bearer"];
                var isValidToken = _tokenService.ValidateToken(token);

                if (!isValidToken)
                {
                    context.Response.Redirect("/Home/Index");
                    return;
                }

                context.Request.Headers.Add("Authorization", $"Bearer {token}");
            }

            await next(context);
        }
    }
}
