using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Repositories;

public interface IUrlShortenerRepository
{
    Task CreateShortUrlAsync(Url url);
    Task<bool> ExistsAsync(string code);
}