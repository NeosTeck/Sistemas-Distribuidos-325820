using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuelistApi.Infrastructure.Documents;

public class DuelistDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("nickname")]
    public string Nickname { get; set; }

    [BsonElement("series")]
    public string Series { get; set; }

    [BsonElement("archetype")]
    public string Archetype { get; set; }

    [BsonElement("signature_card")]
    public string SignatureCard { get; set; }

    [BsonElement("favorite_mechanic")]
    public string FavoriteMechanic { get; set; }
}
