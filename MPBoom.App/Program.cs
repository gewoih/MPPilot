using BlazorBootstrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MPBoom.App.Infrastructure.Contexts;
using MPBoom.App.Middleware;
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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

#if DEBUG
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
#endif

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

#if DEBUG
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
#endif
            }
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