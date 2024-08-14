using UrlShortener.Domain.Utils;

namespace UrlShortener.Domain.Entities;

public class Url(string fullUrl)
{
    public int Id { get; private set; }
    public string Code { get; private set; } = CodeGenerator.GenerateRandomCode(length: 7);
    public string FullUrl { get; private set; } = fullUrl;
    public DateTime CreateAt { get; private set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddSeconds(120);
}