using Microsoft.AspNetCore.Mvc;
using UrlShortening.Shared.Models;

namespace UrlShorteningApi
{
	public static class UrlShorteningResponseHelper
	{
		public static async Task<IActionResult> GenerateResponse(this Task<UrlShorteningResponse> result)
		{
			UrlShorteningResponse response = await result;

			if (response.HasError)
			{
				return new BadRequestObjectResult(response);
			}

			return new OkObjectResult(response);
		}
	}
}
