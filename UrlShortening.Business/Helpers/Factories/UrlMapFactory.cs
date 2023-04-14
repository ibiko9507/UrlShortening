using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.Abstractions;
using UrlShortening.Shared.Models;

namespace UrlShortening.Business.Helpers.Factories
{

	public class UrlMapFactory : IUrlMapFactory
	{
		public UrlMap CreateUrlMap(string longUrl, string shortUrl)
		{
			return new UrlMap
			{
				LongUrl = longUrl,
				ShortUrl = shortUrl
			};
		}
	}
}
