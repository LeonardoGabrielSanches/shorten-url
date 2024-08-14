using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Repositories;

public interface IUrlShortenerRepository
{
    Task CreateShortUrlAsync(Url url);
    Task<bool> ExistsByCodeAsync(string code);
    Task<Url?> GetUrlByCodeAsync(string code);
}