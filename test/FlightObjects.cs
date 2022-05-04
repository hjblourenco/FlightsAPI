using System;
using System.Linq;

public class FlightObject {

    public static Flight CreateTestFlightObject()
    {
        var flight = new Flight()
        {   FlightId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            AirlineId = Guid.NewGuid(),
            FlightNumber = RandomString(10),
            ReservationCode = "ABC123",
            Fare = "Standard",
            DepartureAirport = "LHR",
            DepartureCountry = "UK",                
            ArrivalAirport = "AMS",
            ArrivalCountry = "NL",
            DepartureDate = DateTime.Now,
            ArrivalDate = DateTime.Now.AddHours(1),
            Price = 100,
            PricePaid = 80,
            PaymentType = PaymentType.Cash,

            UsePrice = true,
            Website = "http://www.google.com",
            Notes = "This is a test flight"
        };
        return flight;
    }

    private static Random random = new Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}