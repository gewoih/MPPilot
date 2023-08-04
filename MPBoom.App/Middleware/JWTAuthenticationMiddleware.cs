namespace MPBoom.App.Middleware
{
    public class JWTAuthenticationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (RequiresAuthentication(context))
            {
                string authorizationHeader = context.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    string token = authorizationHeader["Bearer ".Length..].Trim();

                    //if (IsValidToken(token))
                    //    await _next(context);
                    //else
                    //    context.Response.Redirect("/login");
                }
                else
                {
                    context.Response.Redirect("/login");
                }
            }
            else
                await next(context);
        }

        private static bool RequiresAuthentication(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(new PathString("/secure"));
        }
    }
}
