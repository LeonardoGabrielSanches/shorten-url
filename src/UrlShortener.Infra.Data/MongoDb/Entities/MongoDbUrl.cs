using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infra.Data.MongoDb.Entities;

public class MongoDbUrl
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    private string MongoId { get; init; } = null!;

    [BsonRepresentation(BsonType.String)] public string Code { get; private init; } = null!;

    [BsonRepresentation(BsonType.String)] private string OriginalUrl { get; init; } = null!;

    [BsonRepresentation(BsonType.DateTime)]
    private DateTime CreateAt { get; init; }

    [BsonRepresentation(BsonType.DateTime)]
    private DateTime ExpiresAt { get; init; }

    public static implicit operator MongoDbUrl(Url url)
        => new()
        {
            MongoId = url.Id,
            Code = url.Code,
            OriginalUrl = url.OriginalUrl,
            CreateAt = url.CreateAt,
            ExpiresAt = url.ExpiresAt
        };

    public static implicit operator Url(MongoDbUrl? mongoDbUrl)
    {
        return mongoDbUrl is null
            ? null!
            : new Url(mongoDbUrl.MongoId, mongoDbUrl.OriginalUrl, mongoDbUrl.Code, mongoDbUrl.CreateAt,
                mongoDbUrl.ExpiresAt);
    }
}