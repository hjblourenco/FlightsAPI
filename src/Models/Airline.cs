using MongoDB.Bson.Serialization.Attributes;

public class Airline
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid AirlineId { get; set; }

    [BsonElement("Name")]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    [BsonRequired]
    public string Name { get; set; }


}