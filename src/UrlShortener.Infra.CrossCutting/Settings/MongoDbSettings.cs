namespace UrlShortener.Api.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; init; } = null!;
    public string Database { get; init; } = null!;
}