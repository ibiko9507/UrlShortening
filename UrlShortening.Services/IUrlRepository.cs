using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.Shared.Models;

namespace UrlShortening.Abstractions
{
	public interface IUrlRepository
	{
		Task<string?> GetLongUrlAsync(string shortUrl);
		Task<string?> GetShortUrlAsync(string longUrl);
		Task<bool> DeleteUrlAsync(string shortUrl);
		Task AddUrlMap(UrlMap urlMap);
	}
}
