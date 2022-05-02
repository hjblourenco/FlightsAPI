public interface IFlightsRepository
{
    public IEnumerable<Flight> GetAllFlights();

    public Flight GetFlightById(Guid flightId);

    public void AddFlight(Flight flight);

    void UpdateFlight(Guid flightId,Flight flight);
    public void DeleteFlight(Guid flightId);

    public bool FlightExists(Guid flightId);

    public bool Save();

    public IEnumerable<Flight> GetFlightsByUserId(Guid userId);
    
}