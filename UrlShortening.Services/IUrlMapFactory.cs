using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortening.Shared.Models;

namespace UrlShortening.Abstractions
{
	public interface IUrlMapFactory
	{
		UrlMap CreateUrlMap(string longUrl, string shortUrl);
	}
}
