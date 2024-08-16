using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UrlShortener.Api.Settings;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Repositories;
using UrlShortener.Infra.Data.MongoDb.Entities;

namespace UrlShortener.Infra.Data.MongoDb.Repositories;

public class MongoDbUrlRepository : IUrlShortenerRepository
{
    private const string COLLECTION_NAME = "urls";
    private readonly IMongoCollection<MongoDbUrl> _mongoUrlsCollection;

    public MongoDbUrlRepository(IOptions<MongoDbSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.Database);

        _mongoUrlsCollection = mongoDatabase.GetCollection<MongoDbUrl>(COLLECTION_NAME);
    }

    public async Task CreateShortUrlAsync(Url url)
        => await _mongoUrlsCollection.InsertOneAsync(url);

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        var result = await _mongoUrlsCollection.Find(x => x.Code == code).FirstOrDefaultAsync();

        return result is not null;
    }

    public async Task<Url?> GetUrlByCodeAsync(string code)
    {
        var result = await _mongoUrlsCollection.Find(x => x.Code == code).FirstOrDefaultAsync();

        return result;
    }
}