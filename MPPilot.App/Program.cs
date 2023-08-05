using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MPBoom.Domain.Models.Token;
using MPBoom.Domain.Services.API;
using MPBoom.Domain.Services.Security.Token;
using MPPilot.App.Infrastructure;
using MPPilot.App.Middleware;
using MPPilot.App.Services;

namespace MPPilot.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllersWithViews();

			var connectionString = builder.Configuration.GetConnectionString("Default");
			builder.Services.AddDbContext<MPBoomContext>(options => options.UseNpgsql(connectionString));
			builder.Services.AddHttpClient();

			AuthOptions.SetKey(builder.Configuration);
			builder.Services.AddSingleton<ITokenService, JWTTokenService>();
			builder.Services.AddScoped<WildberriesService>();
			builder.Services.AddScoped<AccountsService>();

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
				});

			builder.Services.AddAuthorization();

			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseMiddleware<JWTAuthenticationMiddleware>();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapDefaultControllerRoute();

			app.Run();
		}
	}
}