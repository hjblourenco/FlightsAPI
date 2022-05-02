public interface IAirlinesRepository
{
    public Task<Airline> GetAirlineAsync(Guid airlineId);

    public Task<IEnumerable<Airline>> GetAirlineByNameAsync(string name);

    public void AddAirlineAsync(Airline airline);

    public void UpdateAirlineAsync(Guid airlineId,Airline airline);

    public bool AirlineExistsAsync(Guid airlineId);

    public Task<bool> AirlineExistsByNameAsync(string name);

    public Task<IEnumerable<Airline>> GetAllAirlinesAsync();

    public void DeleteAirlineAsync(Guid airlineId);

    public bool Save();
}