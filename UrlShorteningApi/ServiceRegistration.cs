using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.IO;
using UrlShortening.Abstractions;
using UrlShortening.Business.Helpers.Factories;
using UrlShortening.Business.Services;
using UrlShortening.DataRepository;

namespace UrlShortening.Api
{
	public static class ServiceRegistration
	{
		public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = "DOCKER_HOST_IP:6379,abortConnect=false";
			});

			services.AddScoped<CustomUrlValidator>();
			services.AddScoped<UrlShorteningValidator>();

			services.AddSingleton<IDatabase>((provider) =>
			{
				var connectionString = configuration.GetConnectionString("RedisConnection");
				var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
				return connectionMultiplexer.GetDatabase();
			});

			services.AddScoped<IUrlRepository, UrlRepository>((provider) =>
			{
				var connectionString = configuration.GetConnectionString("RedisConnection");
				return new UrlRepository(connectionString);
			});

			services.AddScoped<IUrlMapFactory, UrlMapFactory>();
			services.AddScoped<IUrlShorteningService, UrlShorteningService>();
		}
	}
}
