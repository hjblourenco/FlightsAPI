public class FlightUpdateDto
{

    //MongoDb and the database dont generate the main ID, ans i'm not using another framework I have to generate it
    public Guid FlightId { get; set; } 
    public Guid UserId { get; set; }
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