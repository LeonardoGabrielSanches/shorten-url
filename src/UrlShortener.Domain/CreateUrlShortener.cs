using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Repositories;

namespace UrlShortener.Domain;

public sealed class CreateUrlShortener(IUrlShortenerRepository urlShortenerRepository)
{
    public record CreateUrlShortenerRequest(string Url);

    public record CreateUrlShortenerResponse(string ShortUrl);

    public async Task<CreateUrlShortenerResponse> Handle(CreateUrlShortenerRequest request)
    {
        var url = new Url(request.Url);

        var shortUrl = await urlShortenerRepository.CreateShortUrlAsync(url);

        return new CreateUrlShortenerResponse(shortUrl);
    }
}