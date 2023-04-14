using FluentValidation;
using FluentValidation.Results;
using UrlShortening.Abstractions;
using UrlShortening.DataRepository;
using UrlShortening.Shared.Constants;
using UrlShortening.Shared.Models;

public class UrlShorteningValidator : AbstractValidator<string>, IUrlShorteningValidator
{
	private readonly IUrlRepository _urlRepository;

	public UrlShorteningValidator(IUrlRepository urlRepository)
	{
		_urlRepository = urlRepository;

		RuleFor(longUrl => longUrl)
			.NotEmpty()
				.WithMessage(UserMessageConstants.UrlCanNotBeEmpty)
			.MustAsync(async (longUrl, cancellation) => !await IsLongUrlExists(longUrl))
				.WithMessage(UserMessageConstants.TheUrlIsInUsage)
			.Must(BeValidUrl)
				.WithMessage(UserMessageConstants.EnterValidUrl);
	}

	private bool BeValidUrl(string longUrl)
	{
		return Uri.TryCreate(longUrl, UriKind.Absolute, out Uri result)
			   && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
	}

	private async Task<bool> IsLongUrlExists(string longUrl)
	{
		var shortUrl = await _urlRepository.GetLongUrlAsync(longUrl);
		return !string.IsNullOrEmpty(shortUrl);
	}

	private async Task<bool> IshortUrlExists(string shortUrl)
	{
		var longUrl = await _urlRepository.GetLongUrlAsync(shortUrl);
		return !string.IsNullOrEmpty(longUrl);
	}

}
