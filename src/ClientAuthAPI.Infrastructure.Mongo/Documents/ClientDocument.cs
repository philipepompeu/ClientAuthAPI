using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClientAuthAPI.Infrastructure.Mongo.Documents;

public class ClientDocument{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [BsonElement("client_secret")]
    public string ClientSecret { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
}