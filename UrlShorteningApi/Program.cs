using Microsoft.Extensions.Hosting;
using UrlShortening.Api;
using VotingAPI.WebAPI.Filters;

namespace UrlShortening
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.Build();

			builder.Services.RegisterServices(configuration);
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			//app.UseMiddleware<GlobalHandlerMiddleware>();
			app.MapControllers();
			app.Run();
		}
	}
}
