using FluentValidation;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UrlShortening.Abstractions;
using UrlShortening.Business.Helpers.Factories;
using UrlShortening.Shared.Constants;
using UrlShortening.Shared.Models;

namespace UrlShortening.Business.Services
{
	public class UrlShorteningService : IUrlShorteningService
	{
		private static readonly Random Random = new Random();
		private readonly CustomUrlValidator _customUrlValidator;
		private readonly UrlShorteningValidator _urlShorteningValidator;
		private readonly IUrlRepository _urlRepository;
		private readonly IUrlMapFactory _urlMapFactory;

		public UrlShorteningService(
			CustomUrlValidator customUrlValidator,
			UrlShorteningValidator urlShorteningValidator,//todo : validator kullanımlarını tekilleştir // base bir yapı kur
			IUrlRepository urlRepository,
			IDatabase redisConnectionProvider,
			IUrlMapFactory urlMapFactory
			)
		{
			_urlRepository = urlRepository;
			_customUrlValidator = customUrlValidator;
			_urlMapFactory = urlMapFactory;
			_urlShorteningValidator = urlShorteningValidator;
		}

		public async Task<UrlShorteningResponse> GetShortUrl(string originalUrl)
		{
			var validationResult = await _urlShorteningValidator.ValidateAsync(originalUrl);
			if (!validationResult.IsValid)
			{
				return new UrlShorteningResponse
				{
					ResponseMessage = validationResult.Errors,
					HasError = true,
				};
			}

			var shortUrl = ConvertUrlToShortenedUrl(originalUrl);
			await MapLongUrlToShortUrl(originalUrl, shortUrl);

			return new UrlShorteningResponse
			{
				ResponseMessage = UserMessageConstants.ShortUrlSuccessfullyCreated,
				HasError = false,
			};
		}

		private string ConvertUrlToShortenedUrl(string url) //Could be used some frameworks instead
		{
			Uri uri = new Uri(url);
			string host = uri.Host;
			string path = uri.AbsolutePath;
			int hashLen = Random.Next(1, UrlShorteningConstant.DefaultHashLength + 1); //maximum 6 character
			string shortUrl = $"{uri.Scheme.Replace(Uri.UriSchemeHttps, Uri.UriSchemeHttp)}://{RemoveSubDomains(host)}/{GenerateRandomString(hashLen)}";
			return shortUrl;
		}

		public async Task<UrlShorteningResponse> PickCustomizedShortUrl(string originalUrl, string customUrl)
		{
			var validationResult = await _customUrlValidator.ValidateAsync(_urlMapFactory.CreateUrlMap(originalUrl, customUrl));

			if (!validationResult.IsValid)
			{
				return new UrlShorteningResponse
				{
					ResponseMessage = validationResult.Errors,
					HasError = true,
				};
			}
			MapLongUrlToShortUrl(originalUrl, customUrl);
			return new UrlShorteningResponse
			{
				ResponseMessage = UserMessageConstants.CustomUrlSuccessfullyCreated,
				HasError = false,
				ShortUrl = customUrl,
				LongUrl = originalUrl
			};
		}

		public async Task<UrlShorteningResponse> GetLongUrl(string shortUrl)
		{
			string longUrl = await _urlRepository.GetLongUrlAsync(shortUrl);

			if (string.IsNullOrEmpty(longUrl))
			{
				return new UrlShorteningResponse // burayı validator class'ında hallet
				{
					ResponseMessage = UserMessageConstants.LongUrlCouldntFoundInTheDataBase,
					HasError = true,
				};
			}
			return new UrlShorteningResponse
			{
				ResponseMessage = UserMessageConstants.LongUrlHasFoundInTheDataBase,
				HasError = false,
				ShortUrl = shortUrl,
				LongUrl = longUrl
			};
		}
		private string GenerateRandomString(int length)
		{
			char[] result = new char[length];
			for (int i = 0; i < length; i++)
			{
				result[i] = UrlShorteningConstant.AllowedCharacters[Random.Next(UrlShorteningConstant.AllowedCharacters.Length)];
			}
			return new string(result);
		}

		public async Task MapLongUrlToShortUrl(string longUrl, string shortUrl)
		{
			var urlMap = _urlMapFactory.CreateUrlMap(longUrl, shortUrl);
			await _urlRepository.AddUrlMap(urlMap);
		}

		private string RemoveSubDomains(string url)
		{
			string[] subDomainsToRemove = new string[] { "www.", "blog." }; // Örnek alt alan adları
			foreach (var subDomain in subDomainsToRemove)
			{
				url = url.Replace(subDomain, string.Empty);
			}
			return url;
		}
	}
}
