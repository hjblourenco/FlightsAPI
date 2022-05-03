public class FlightSearchDto
{


    //We need the keys to connect to the main model but hte comparision is made with strings
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
    public string DepartureDate { get ; set; } = String.Empty;
    public string ArrivalDate { get; set; } = String.Empty;
    public string Price { get; set; } = String.Empty;
    public string PricePaid { get; set; } = String.Empty;
    public string PaymentType { get ; set; } = String.Empty;    

}