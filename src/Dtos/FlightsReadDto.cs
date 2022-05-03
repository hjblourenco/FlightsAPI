public class FlightReadDto
{

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
    public PaymentType PaymentType { get ; set; }    

}