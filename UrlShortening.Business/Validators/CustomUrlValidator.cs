using FluentValidation;
using FluentValidation.Results;
using UrlShortening.Abstractions;
using UrlShortening.DataRepository;
using UrlShortening.Shared.Constants;
using UrlShortening.Shared.Models;

public class CustomUrlValidator : AbstractValidator<UrlMap>, ICustomUrlValidator
{
	private readonly IUrlRepository _urlRepository;

	public CustomUrlValidator(IUrlRepository urlRepository)
	{
		_urlRepository = urlRepository;

		RuleFor(customUrl => customUrl)
			.NotEmpty().WithMessage(UserMessageConstants.UrlCanNotBeEmpty)
			.Must(BeValidUrl)
				.WithMessage(UserMessageConstants.EnterValidUrl)
			.MustAsync(async (customUrl, cancellation) => !await IsShortUrlExists(customUrl))
				.WithMessage(UserMessageConstants.TheCustomUrlIsInUsage)
			.MustAsync(async (customUrl, cancellation) => !await IsLongUrlExists(customUrl))
				.WithMessage(UserMessageConstants.TheLongUrlIsInUsage);
	}

	private bool BeValidUrl(UrlMap urlMap)
	{
		return Uri.TryCreate(urlMap.ShortUrl, UriKind.Absolute, out Uri resultShort)
			   && (resultShort.Scheme == Uri.UriSchemeHttp || resultShort.Scheme == Uri.UriSchemeHttps)
			   && Uri.TryCreate(urlMap.LongUrl, UriKind.Absolute, out Uri resultLong)
			   && (resultLong.Scheme == Uri.UriSchemeHttp || resultLong.Scheme == Uri.UriSchemeHttps);
	}

	private async Task<bool> IsLongUrlExists(UrlMap urlMap)
	{
		var shortUrl = await _urlRepository.GetShortUrlAsync(urlMap.LongUrl);
		return !string.IsNullOrEmpty(shortUrl);
	}

	public async Task<bool> IsShortUrlExists(UrlMap urlMap)
	{
		var longUrl = await _urlRepository.GetLongUrlAsync(urlMap.ShortUrl);
		return !string.IsNullOrEmpty(longUrl);
	}

	public bool IsCustomUrlValid(UrlMap urlMap)
	{
		var validationResult = Validate(urlMap);
		return validationResult.IsValid;
	}

}
