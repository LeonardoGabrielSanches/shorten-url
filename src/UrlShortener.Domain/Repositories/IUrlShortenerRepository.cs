using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Repositories;

public interface IUrlShortenerRepository
{
    Task<string> CreateShortUrlAsync(Url url);
}