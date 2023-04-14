
using StackExchange.Redis;
using UrlShortening.Shared.Constants;
using UrlShortening.Shared.Models;

namespace UrlShortening.DataRepository
{
	public static class HashSetGenerator
	{
		public static HashEntry[] CreateHash(this UrlMap urlMap)
		{
			HashEntry[] hashEntries = new HashEntry[2];
			hashEntries[0] = new HashEntry(urlMap.LongUrl, urlMap.ShortUrl);
			hashEntries[1] = new HashEntry(urlMap.ShortUrl, urlMap.LongUrl);

			return hashEntries;
		}
	}
}