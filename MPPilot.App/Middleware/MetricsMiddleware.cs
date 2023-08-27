using System.Diagnostics.Metrics;

namespace MPPilot.App.Middleware
{
	public class MetricsMiddleware : IMiddleware
	{
		private readonly Meter _meter;

		public MetricsMiddleware(IMeterFactory meterFactory)
		{
			_meter = meterFactory.Create("MPPilot");
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var requestPath = context.Request.Path.ToString();
			var statusCode = context.Response.StatusCode.ToString();

			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				var counter = _meter.CreateCounter<int>("http_exceptions");
				counter.Add(1, new KeyValuePair<string, object>("type", ex.GetType().Name),
								new KeyValuePair<string, object>("path", requestPath));
			}
			finally
			{
				if (!requestPath.Contains("metrics"))
				{
					var counter = _meter.CreateCounter<int>("http_requests_total");
					counter.Add(1, KeyValuePair.Create<string, object>("path", requestPath), KeyValuePair.Create<string, object>("status_code", statusCode));
				}
			}
		}
	}
}
