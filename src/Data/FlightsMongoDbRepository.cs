using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

public class FlightsMongoDbRepository : IFlightsRepository
{

    private readonly IMongoCollection<BsonDocument> _context;

    // public FlightsMongoDbRepository()
    // {
    // }

    public FlightsMongoDbRepository(MongoDbContext context)
    {

        _context = context._flightsCollection;

    }

    
    public void AddFlight(Flight flight)
    {
        if (flight == null)
        {
            throw new ArgumentNullException(nameof(flight));
        }

        
        //MongoDb and the database dont generate the main ID, ans i'm not using another framework I have to generate it

        _context.InsertOne(flight.ToBsonDocument());

    }

    public void DeleteFlight(Guid flightId)
    {
        Console.WriteLine("--> DeleteFlight repository");
        if (!FlightExists(flightId))
        {
            throw new ArgumentException("Flight does not exist");
        }

        var filter = Builders<BsonDocument>.Filter.Eq("_id", flightId.ToString());
        var flightDeleteResult = _context.FindOneAndDelete(filter);

        Console.WriteLine(flightDeleteResult);
    }

    public bool FlightExists(Guid flightId)
    {
        Console.WriteLine("--> FlightExists repository");
        var filter = Builders<BsonDocument>.Filter.Eq("_id", flightId.ToString());
        var flightExists = _context.Find(filter).ToList().Count()>0;

        return flightExists;
    }

    public IEnumerable<Flight> GetAllFlights()
    {
        Console.WriteLine("--> GetAllFlights repository");
        var documents =  _context.Find(new BsonDocument()).ToList();
        var documentsDeserialized = documents.Select(value => BsonSerializer.Deserialize<Flight>(value));
        
        return documentsDeserialized;
    }

    public Flight? GetFlightById(Guid flightId)
    {
        Console.WriteLine("--> GetFlightById repository");   
        if (!FlightExists(flightId))
        {
            return null;
        }

        //I have to change from guid To String to work with mongo
        var filter = Builders<BsonDocument>.Filter.Eq("_id", flightId.ToString());
        var flight = _context.Find(filter).First();

        var flightDeserialized = BsonSerializer.Deserialize<Flight>(flight.ToBsonDocument());
        return flightDeserialized;
    }

    public IEnumerable<Flight> GetFlightsByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public bool Save()
    {
        //With mongodb there is no need to save
        return true;
    }

    public void UpdateFlight(Guid flightId,Flight flight)
    {
        Console.WriteLine("--> UpdateFlight repository");
        if (!FlightExists(flightId))
        {
            throw new ArgumentException("--->Flight does not exist");
        }

        var filter = Builders<BsonDocument>.Filter.Eq("_id", flightId.ToString());
        var flightUpdateResult = _context.FindOneAndReplace(filter, flight.ToBsonDocument());
        //var flightUpdateResult = _context.UpdateOne(filter, flight.ToBsonDocument());


        Console.WriteLine("--> UpdateFlight DONE");
    }
}
