using BlazorBootstrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MPBoom.App.Infrastructure.Contexts;
using MPBoom.App.Services;
using MPBoom.Domain.Models.Token;
using MPBoom.Domain.Services.API;
using MPBoom.Domain.Services.LocalStorage;
using MPBoom.Domain.Services.Security.Token;

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

            builder.Services.AddScoped<JWTAuthenticationStateProvider>();

            AuthOptions.SetKey(builder.Configuration);
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<MPBoomContext>(options => options.UseNpgsql(connectionString));

            builder.Services.AddScoped<AccountsService>();
			builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddScoped<ITokenService, JWTTokenService>();
            builder.Services.AddSingleton<WildberriesService>();

            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
            });

            builder.WebHost.UseUrls("http://localhost:5050", "https://localhost:5051");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

#if DEBUG
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MPBoomAPI", Version = "v1" });
            });
#endif
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ValidJWTOnly", policy =>
                    policy.RequireAssertion(context => 
                        context.User.HasClaim(c => c.Type == "Token")));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

#if DEBUG
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MPBoomAPI");
                });
#endif
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}