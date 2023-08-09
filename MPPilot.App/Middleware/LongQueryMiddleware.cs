using System.Diagnostics;

namespace MPPilot.App.Middleware
{
	public class LongQueryMiddleware : IMiddleware
	{
		private readonly ILogger<LongQueryMiddleware> _logger;
		private readonly double _thresholdSeconds;

		public LongQueryMiddleware(ILogger<LongQueryMiddleware> logger, IConfiguration configuration)
		{
			_logger = logger;
			_thresholdSeconds = double.Parse(configuration["LongQueryThresholdSeconds"]);
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var watch = Stopwatch.StartNew();
			await next(context);
			watch.Stop();

			if (watch.Elapsed.TotalSeconds > _thresholdSeconds)
				LogLongQuery(context, watch.Elapsed);
		}

		private void LogLongQuery(HttpContext context, TimeSpan elapsed)
		{
			var request = context.Request;
			var methodType = request.Method;
			var path = request.Path;
			var controllerName = context.GetRouteValue("controller")?.ToString();
			var actionName = context.GetRouteValue("action")?.ToString();

			_logger.LogWarning("Long query detected in {Controller}Controller/{Action}: [{Method}] {Path} in {Elapsed}", controllerName, actionName, methodType, path, elapsed);
		}
	}
}
