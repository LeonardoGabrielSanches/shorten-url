namespace UrlShortener.Domain.Entities;

public class Url(string originalUrl, string code)
{
    public int Id { get; private set; }
    public string Code { get; private set; } = code;
    public string OriginalUrl { get; private set; } = originalUrl;
    public DateTime CreateAt { get; private set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddSeconds(120);
}