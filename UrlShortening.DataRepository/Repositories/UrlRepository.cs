using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.Abstractions;
using UrlShortening.Shared.Constants;
using UrlShortening.Shared.Models;

namespace UrlShortening.DataRepository
{
	public class UrlRepository : IUrlRepository
	{
		private readonly ConnectionMultiplexer _redisConnection;

		public UrlRepository(string connectionString)
		{
			_redisConnection = ConnectionMultiplexer.Connect(connectionString);
		}

		public async Task<string?> GetLongUrlAsync(string shortUrl)
		{
			var db = _redisConnection.GetDatabase();
			var longUrl = db.HashGetAsync(UrlRepositoryConstants.Shortening, shortUrl);
			return longUrl.Result;
		}

		public async Task<string?> GetShortUrlAsync(string longUrl)
		{
			var db = _redisConnection.GetDatabase();
			return await db.HashGetAsync(UrlRepositoryConstants.Shortening, longUrl);
		}

		public async Task<bool> DeleteUrlAsync(string longUrl)
		{
			var db = _redisConnection.GetDatabase();

			var shortUrl = await db.HashGetAsync(UrlRepositoryConstants.Shortening, longUrl);
			await db.HashDeleteAsync(UrlRepositoryConstants.Shortening, longUrl);
			await db.HashDeleteAsync(UrlRepositoryConstants.Shortening, shortUrl);

			return true;
		}
		public async Task AddUrlMap(UrlMap urlMap)
		{
			var db = _redisConnection.GetDatabase();
			db.HashSet(key: UrlRepositoryConstants.Shortening, hashFields: urlMap.CreateHash());
		}
	}
}
