namespace UrlShortener.Domain.Entities;

public class Url(string originalUrl, string code)
{
    public Url(
        string id,
        string originalUrl,
        string code,
        DateTime createAt,
        DateTime expiresAt) :
        this(originalUrl, code)
    {
        Id = id;
        CreateAt = createAt;
        ExpiresAt = expiresAt;
    }

    public string Id { get; private set; } = null!;
    public string Code { get; private set; } = code;
    public string OriginalUrl { get; private set; } = originalUrl;
    public DateTime CreateAt { get; private set; } = DateTime.Now;
    public DateTime ExpiresAt { get; private set; } = DateTime.Now.AddSeconds(120);
}