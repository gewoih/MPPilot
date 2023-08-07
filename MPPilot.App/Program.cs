﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MPPilot.App.Middleware;
using MPPilot.Domain.Infrastructure;
using MPPilot.Domain.Models.Auth;
using MPPilot.Domain.Services;
using MPPilot.Domain.Services.Autobidders;
using MPPilot.Domain.Services.Token;
using System.Text;

namespace MPPilot.App
{
    public class Program
	{
		public static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;

			var builder = WebApplication.CreateBuilder(args);

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
			builder.Services.AddSingleton<AutobiddersManager>();

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

			var autobiddersManager = app.Services.GetRequiredService<AutobiddersManager>();
			autobiddersManager.StartManagement();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseMiddleware<JWTAuthenticationMiddleware>();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapDefaultControllerRoute();

			app.Run();
		}
	}
}