using Microsoft.EntityFrameworkCore;
using MPBoom.API.Infrastructure.Contexts;
using MPBoom.API.Services;
using MPBoom.Domain.Models.Token;
using MPBoom.Domain.Services;
using System.Text;

namespace MPBoom.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			Console.OutputEncoding = Encoding.UTF8;

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddHttpClient();

			AuthOptions.SetKey(builder.Configuration);
			var connectionString = builder.Configuration.GetConnectionString("Default");
			builder.Services.AddDbContext<MPBoomContext>(options => options.UseNpgsql(connectionString));

			builder.Services.AddScoped<AccountsService>();
			builder.Services.AddScoped<AdvertsBidService>();
			builder.Services.AddScoped<WildberriesService>();

            builder.WebHost.UseUrls("http://localhost:5050", "https://localhost:5051");

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			//app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}