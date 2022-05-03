public class FlightUpdateDto
{

    //MongoDb and the database dont generate the main ID, ans i'm not using another framework I have to generate it
    public Guid FlightId { get; set; } 
    public Guid UserId { get; set; }
    public Guid AirlineId { get; set; }
    public string? FlightNumber { get; set; }
    public string? ReservationCode { get; set; }
    public string Fare { get; set; } = String.Empty;
    public string DepartureAirport { get; set; } = String.Empty;
    public string DepartureCountry { get; set; } = String.Empty;
    public string ArrivalAirport { get; set; } = String.Empty;
    public string ArrivalCountry { get; set; } = String.Empty;
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public Decimal Price { get; set; }
    public Decimal PricePaid { get; set; }
    public PaymentType PaymentType { get; set; }    
    public bool UsePrice { get; set; }   
    public string Website { get; set; } = String.Empty;
    public string Notes { get; set; }= String.Empty;

}