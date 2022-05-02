public class FlightSearchDto
{


    //We need the keys to connect to the main model but hte comparision is made with strings
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
    public string DepartureDate { get ; set; }
    public string ArrivalDate { get; set; }
    public string Price { get; set; }
    public string PricePaid { get; set; }
    public string PaymentType { get ; set; }    

}