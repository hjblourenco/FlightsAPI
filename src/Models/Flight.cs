using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Flight
{

    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid FlightId { get; set; }
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid UserId { get; set; }
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid AirlineId { get; set; }
    public string? FlightNumber { get; set; }
    public string? ReservationCode { get; set; }
    public string Fare { get; set; }
    public string DepartureAirport { get; set; }
    public string DepartureCountry { get; set; }
    public string ArrivalAirport { get; set; }
    public string ArrivalCountry { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public Decimal Price { get; set; }
    public Decimal PricePaid { get; set; }
    public PaymentType PaymentType { get; set; }    
    
    public bool UsePrice { get; set; }   

    public string Website { get; set; }

    public string Notes { get; set; }

}

