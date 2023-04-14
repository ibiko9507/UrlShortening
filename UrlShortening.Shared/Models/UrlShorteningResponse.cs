using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortening.Shared.Models
{
	public class UrlShorteningResponse
	{
		public object? ResponseMessage { get; set; }
		public bool HasError { get; set; }
		public string? ShortUrl { get; set; }
		public string? LongUrl { get; set; }
	}
}
