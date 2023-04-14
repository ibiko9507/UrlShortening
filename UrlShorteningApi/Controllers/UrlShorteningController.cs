using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using UrlShortening.Abstractions;
using UrlShortening.Business.Services;
using UrlShortening.Shared.Models;

namespace UrlShorteningApi
{
	[ApiController]
	[Route("[controller]")]
	public class UrlShorteningController : ControllerBase
	{
		private readonly ILogger<UrlShorteningController> _logger;
		private readonly IUrlShorteningService _urlShorteningService;

		public UrlShorteningController(ILogger<UrlShorteningController> logger, IUrlShorteningService urlShorteningService)
		{
			_logger = logger; 
			_urlShorteningService = urlShorteningService;
		}

		[HttpGet("GetShortUrl")]
		public async Task<IActionResult> GetShortUrl(string originalUrl)
		{
			Task<UrlShorteningResponse> result = _urlShorteningService.GetShortUrl(originalUrl);
			return await result.GenerateResponse();
		}

		[HttpGet("GetLongUrl")]
		public async Task<IActionResult> GetLongUrl(string shortUrl)
		{
			Task<UrlShorteningResponse> result = _urlShorteningService.GetLongUrl(shortUrl);
			return await result.GenerateResponse();
		}

		[HttpGet("PickCustomizedShortUrl")]
		public async Task<IActionResult> PickCustomizedShortUrl(string originalUrl, string customUrl)
		{
			Task<UrlShorteningResponse> result = _urlShorteningService.PickCustomizedShortUrl(originalUrl, customUrl);
			return await result.GenerateResponse();
		}		
	}
}