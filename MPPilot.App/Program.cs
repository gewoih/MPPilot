using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MPPilot.App.Middleware;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Auth;
using MPPilot.Domain.Services.Token;
using System.Text;
using Serilog;
using MPPilot.Domain.Services.Autobidders;
using MPPilot.Domain.Services.Marketplaces;
using MPPilot.Domain.Services.Accounts;
using MPPilot.Domain.BackgroundServices;

namespace MPPilot.App
{
    public class Program
	{
		public static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;

			var builder = WebApplication.CreateBuilder(args);

			builder.Host.UseSerilog((context, configuration) =>
				configuration.ReadFrom.Configuration(context.Configuration)
				.WriteTo.Console());

			builder.Services.AddLogging(builder =>
			{
				builder.AddSerilog();
			});

			builder.Services.AddControllersWithViews();

			var connectionString = builder.Configuration.GetConnectionString("Default");
			builder.Services.AddDbContext<MPPilotContext>(options => options.UseNpgsql(connectionString));
			builder.Services.AddHttpClient();
			builder.Services.AddHttpContextAccessor();

			AuthOptions.SetKey(builder.Configuration);
			builder.Services.AddSingleton<ITokenService, JWTTokenService>();
			builder.Services.AddScoped<AccountsService>();
			builder.Services.AddScoped<AutobidderService>();
			builder.Services.AddScoped<WildberriesService>();
			builder.Services.AddSingleton<AdvertsMarketService>();

			builder.Services.AddScoped<AuthenticationMiddleware>();
			builder.Services.AddScoped<LongQueryMiddleware>();
			builder.Services.AddScoped<ExceptionsHandlerMiddleware>();

			builder.Services.AddHostedService<AutobiddersManager>();

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = AuthOptions.Issuer,
						ValidAudience = AuthOptions.Audience,
						IssuerSigningKey = AuthOptions.GetSecurityKey()
					};

					options.Events = new JwtBearerEvents
					{
						OnMessageReceived = context =>
						{
							context.Token = context.Request.Cookies[JwtBearerDefaults.AuthenticationScheme];
							return Task.CompletedTask;
						}
					};
				});

			builder.Services.AddAuthorization();

			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseMiddleware<ExceptionsHandlerMiddleware>();
				app.UseMiddleware<LongQueryMiddleware>();

				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseAuthentication();
			app.UseMiddleware<AuthenticationMiddleware>();
			
			app.UseAuthorization();
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