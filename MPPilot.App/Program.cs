using Microsoft.EntityFrameworkCore;
using MPPilot.App.Middleware;
using MPPilot.Domain.Infrastructure;
using System.Text;
using Serilog;
using MPPilot.Domain.Services.Autobidders;
using MPPilot.Domain.Services.Marketplaces;
using MPPilot.Domain.Services.Accounts;
using MPPilot.Domain.BackgroundServices;
using Serilog.Sinks.Elasticsearch;
using OpenTelemetry.Metrics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using MPPilot.Domain.Models.Users;

namespace MPPilot.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;

			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddOpenTelemetry()
				.WithMetrics(builder =>
				{
					builder.AddPrometheusExporter();
					builder.AddAspNetCoreInstrumentation();
					builder.AddPrometheusHttpListener();

					builder.AddMeter("Microsoft.AspNetCore.Hosting",
									 "Microsoft.AspNetCore.Server.Kestrel");

					builder.AddView("http-server-request-duration",
						new ExplicitBucketHistogramConfiguration
						{
							Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05,
								   0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 }
						});
				});

			builder.Host.UseSerilog((context, configuration) =>
				configuration.ReadFrom.Configuration(context.Configuration)
				.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elastic:9200")))
				.WriteTo.Console());

			builder.Services.AddLogging(builder =>
			{
				builder.AddSerilog();
			});

			var connectionString = builder.Configuration.GetConnectionString("Default");
			builder.Services.AddDbContext<MPPilotContext>(options => options.UseNpgsql(connectionString));

			builder.Services.AddAuthentication("Identity.Application")
				.AddBearerToken(IdentityConstants.BearerScheme)
				.AddCookie("Identity.Application", options =>
				{
					options.AccessDeniedPath = "/Account/AccessDenied";
					options.Cookie.Name = "MPPilot";
					options.Cookie.HttpOnly = true;
					options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
					options.LoginPath = "/Account/Login";
					options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
					options.SlidingExpiration = true;
				});
			
			builder.Services.AddAuthorizationBuilder();
			builder.Services.AddIdentityCore<User>(options =>
			{
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 4;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireDigit = false;
			})
				.AddEntityFrameworkStores<MPPilotContext>()
				.AddApiEndpoints();

			builder.Services.AddControllersWithViews();
			builder.Services.AddHttpClient();

			builder.Services.AddHttpContextAccessor();

			builder.Services.AddScoped<IUsersService, UsersService>();
			builder.Services.AddScoped<IAutobiddersService, AutobiddersService>();
			builder.Services.AddScoped<WildberriesService>();
			builder.Services.AddSingleton<AdvertsMarketService>();

			builder.Services.AddScoped<AuthenticationMiddleware>();
			builder.Services.AddScoped<LongQueryMiddleware>();
			builder.Services.AddScoped<ExceptionsHandlerMiddleware>();

			builder.Services.AddHostedService<AutobiddersManager>();

			builder.Services.AddAuthorization();

			builder.WebHost.UseUrls("http://0.0.0.0:80", "https://0.0.0.0:443");

			var app = builder.Build();
			app.MapPrometheusScrapingEndpoint();


			if (!app.Environment.IsDevelopment())
			{
				app.UseMiddleware<ExceptionsHandlerMiddleware>();
				app.UseMiddleware<LongQueryMiddleware>();

				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseAuthentication();
			app.UseAuthorization();
			
			app.UseMiddleware<AuthenticationMiddleware>();

			app.MapDefaultControllerRoute();

			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<MPPilotContext>();
				db.Database.Migrate();
			}

			app.Run();
		}
	}
}