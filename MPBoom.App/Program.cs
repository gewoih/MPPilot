using BlazorBootstrap;
using MPBoom.App.Domain.Interfaces;
using MPBoom.App.Domain.Services;
using MPBoom.Core.Services;

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
			builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddSingleton<WildberriesService>();

            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
            });

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}