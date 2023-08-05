using Microsoft.EntityFrameworkCore;
using MPBoom.Domain.Services.API;
using MPBoom.Domain.Services.Security.Token;
using MPPilot.App.Infrastructure;

namespace MPPilot.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient();

            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<MPBoomContext>(options => options.UseNpgsql(connectionString));

            builder.Services.AddScoped<ITokenService, JWTTokenService>();
            builder.Services.AddScoped<WildberriesService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.MapDefaultControllerRoute();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.Run();
        }
    }
}