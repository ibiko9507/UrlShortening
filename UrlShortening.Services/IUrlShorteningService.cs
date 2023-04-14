using UrlShortening.Shared.Models;

namespace UrlShortening.Abstractions
{
	public interface IUrlShorteningService
	{
		Task<UrlShorteningResponse> GetShortUrl(string originalUrl);
		Task<UrlShorteningResponse> GetLongUrl(string originalUrl);
		Task<UrlShorteningResponse> PickCustomizedShortUrl(string originalUrl, string customUrl);
	}
}