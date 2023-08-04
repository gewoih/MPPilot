using BlazorBootstrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MPBoom.App.Infrastructure.Contexts;
using MPBoom.App.Middleware;
using MPBoom.Domain.Models.Token;
using MPBoom.Domain.Services;
using MPBoom.Domain.Services.LocalStorage;
using MPBoom.Domain.Services.Token;

namespace MPBoom.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
			
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddHttpClient();

            builder.Services.AddTransient<JWTAuthenticationMiddleware>();

            AuthOptions.SetKey(builder.Configuration);
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<MPBoomContext>(options => options.UseNpgsql(connectionString));

			builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddScoped<ITokenService, JWTTokenService>();
            builder.Services.AddSingleton<WildberriesService>();

            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.Issuer,
                            
                            ValidateAudience = true,
                            ValidAudience = AuthOptions.Audience,
                            
                            ValidateLifetime = true,

                            IssuerSigningKey = AuthOptions.GetSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            builder.WebHost.UseUrls("http://localhost:5050", "https://localhost:5051");

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseMiddleware<JWTAuthenticationMiddleware>();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}